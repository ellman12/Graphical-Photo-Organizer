using System.Collections.Generic;
using System.Linq;
using ExifLib;
using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;
using static System.Int32;
using ExifReader = ExifLib.ExifReader;

namespace PSS.Backend
{
    public static class Metadata
    {
        public enum DateTakenSrc
        {
            Metadata,
            Filename,
            Now //DateTime.Now
        }

        //Get the Date Taken for an item, if possible.
        //Return true if date taken was found either in the metadata or filename, or false if using DateTime.Now
        public static bool GetDateTaken(string path, out DateTime dateTaken, out DateTakenSrc dateTakenSrc)
        {
            dateTaken = DateTime.Now;
            dateTakenSrc = DateTakenSrc.Now;

            string ext = Path.GetExtension(path).ToLower(); //Some files might have extension in all caps for no reason.
            bool hasData = ext switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" => GetImgDateTaken(path, out dateTaken, out dateTakenSrc),
                ".mp4" or ".mkv" or ".mov" => GetVideoDateTaken(path, out dateTaken, out dateTakenSrc),
                _ => false
            };

            return hasData;
        }

        //Try and examine metadata. If necessary, it analyzes filename. If can't find data in either, default to DateTime.Now.
        //Returns true if had metadata.
        private static bool GetImgDateTaken(string path, out DateTime dateTaken, out DateTakenSrc src)
        {
            bool hasData;
            try
            {
                ExifReader reader = new(path);

                //I think if this ↓ returns false it means no data found. 0 documentation on this... 
                hasData = reader.GetTagValue(ExifTags.DateTimeDigitized, out dateTaken);

                if (dateTaken == DateTime.MinValue || hasData == false)
                    throw new ExifLibException(); //If GetTagValue returns DateTime.MinValue, means no data found (hasData == false means same thing), so try reading filename instead.

                src = DateTakenSrc.Metadata;
            }
            catch (ExifLibException) //No metadata in file.
            {
                hasData = GetFilenameTimestamp(Path.GetFileName(path), out dateTaken, out src);
                if (!hasData) dateTaken = DateTime.Now;
            }

            return hasData;
        }

        ///<summary>Get when a video file was taken.</summary>
        ///<returns>True if this file had data.</returns>
        private static bool GetVideoDateTaken(string path, out DateTime dateTaken, out DateTakenSrc src)
        {
            dateTaken = DateTime.Now;
            src = DateTakenSrc.Now;
            
            try
            {
                IEnumerable<MetadataExtractor.Directory> directories = QuickTimeMetadataReader.ReadMetadata(new FileStream(path, FileMode.Open));
                QuickTimeMovieHeaderDirectory directory = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();

                if (directory != null && directory.TryGetDateTime(QuickTimeMovieHeaderDirectory.TagCreated, out dateTaken))
                {
                    src = DateTakenSrc.Metadata;
                    return true;
                }
            }
            catch (Exception)
            {
                src = DateTakenSrc.Now;
                bool hasData = GetFilenameTimestamp(Path.GetFileName(path), out dateTaken, out src);
                if (hasData) return true;
                
                dateTaken = DateTime.Now;
                src = DateTakenSrc.Now;
                return false;
            }

            return false;
        }

        //Used if program can't find date/time metadata in the file. Often, filenames will have a timestamp in them.
        //E.g., the Nintendo Switch generates pics/vids filenames like: 2018022016403700_s.mp4. This can be stripped and
        //converted into an actual DateTime object.
        private static bool GetFilenameTimestamp(string filename, out DateTime dateTaken, out DateTakenSrc src)
        {
            bool hasData;
            string timestamp = ""; //The actual timestamp in the filename, without the extra chars we don't want. Converted to DateTime at the end.

            try
            {
                if (filename.StartsWith("Screenshot_")) //If Android screenshot. E.g., 'Screenshot_20201028-141626_Messages.jpg'
                {
                    timestamp = filename.Substring(11, 8) + filename.Substring(20, 6); //Strip the chars we don't want.
                }
                else if (filename.StartsWith("IMG_") || filename.StartsWith("VID_"))
                {
                    timestamp = filename.Substring(4, 8) + filename.Substring(13, 6);
                }
                else if (filename[4] == '-' && filename[13] == '-' && filename[16] == '-' && filename.EndsWith(".mkv")) //Check if an OBS-generated file. It would have '-' at these 3 indices.
                {
                    timestamp = filename;
                    timestamp = filename[..(timestamp.Length - 4)]; //Remove extension https://stackoverflow.com/questions/15564944/remove-the-last-three-characters-from-a-string
                    timestamp = timestamp.Replace("-", "").Replace(" ", "");
                }
                else if (filename[8] == '_' && !filename.StartsWith("messages")) //A filename like this: '20201031_090459.jpg'. I think these come from (Android(?)) phones. Not 100% sure.
                {
                    timestamp = filename[..8] + filename.Substring(9, 6);
                }
                else if (filename.Contains("_s")) //A Nintendo Switch screenshot/video clip, like '2018022016403700_s.mp4'.
                {
                    timestamp = filename[..14];
                }
                else if (filename.StartsWith("Capture") && filename.EndsWith(".png")) //Terraria's Capture Mode 'Capture 2020-05-16 21_04_54.png'
                {
                    timestamp = filename.Substring(8, 19);
                    timestamp = timestamp.Replace("-", "").Replace(":", "").Replace("_", "").Replace(" ", "");
                }
                else if (filename.EndsWith("_1.jpg")) //Not sure if these are exclusive to Terraria or what '20201226213009_1.jpg'
                {
                    timestamp = filename[..14];
                }
                else if (filename.Contains("105600") && filename.EndsWith("_1.png")) //Might just be another Terraria-exclusive thing '105600_20201122143721_1.png'
                {
                    timestamp = filename.Substring(7, 14);
                }
                else if (filename.StartsWith("413150") && filename.EndsWith("_1.png")) //Stardew Valley uncompressed screenshots
                {
                    timestamp = filename.Substring(7, 14);
                }
                else if (filename.StartsWith("Screenshot ")) //Snip & Sketch generates these filenames. E.g., 'Screenshot 2020-11-17 104051.png'
                {
                    timestamp = filename.Substring(11, 17);
                    timestamp = timestamp.Replace("-", "").Replace(" ", "");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in GetFilenameTimestamp() {e.Message}");
            }
            finally
            {
                if (timestamp == "")
                {
                    dateTaken = DateTime.Now;
                    hasData = false;
                    src = DateTakenSrc.Now;
                }
                else
                    hasData = ParseTimestamp(timestamp, out dateTaken, out src);
            }

            return hasData;
        }

        //Try parsing timestamp like this: "20211031155822"
        //Returns false if unable to parse.
        private static bool ParseTimestamp(string timestamp, out DateTime dateTime, out DateTakenSrc src)
        {
            if (timestamp.Length < 14 || DateTime.TryParse(timestamp, out dateTime))
            {
                dateTime = DateTime.Now;
                src = DateTakenSrc.Now;
                return false;
            }

            int year = Parse(timestamp[..4]);
            int month = Parse(timestamp[4..6]);
            int day = Parse(timestamp[6..8]);
            int hour = Parse(timestamp[8..10]);
            int min = Parse(timestamp[10..12]);
            int sec = Parse(timestamp[12..14]);

            dateTime = new DateTime(year, month, day, hour, min, sec);
            src = DateTakenSrc.Filename;
            return true;
        }
    }
}