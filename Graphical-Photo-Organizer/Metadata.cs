using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExifLib;
using MetadataExtractor;
using MetadataExtractor.Formats.QuickTime;
using static System.Int32;
using ExifReader = ExifLib.ExifReader;

namespace Graphical_Photo_Organizer
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
        //Return true if data was found or false if using DateTime.Now
        //Steps:
        //1. Determine type.
        //2. Try reading embedded metadata (if the type is even capable of doing so).
        //3. If no metadata found, try reading filename.
        //4. If all else fails, set it to date time right now.
        public static (bool, DateTakenSrc) GetDateTaken(string path, out DateTime dateTaken)
        {
            bool hasData = false;
            dateTaken = DateTime.Now;
            var src = DateTakenSrc.Now;

            switch (Path.GetExtension(path))
            {
                case ".jpg" or ".jpeg":
                    hasData = GetJpgDate(path, out dateTaken, ref src);
                    break;

                case ".png":
                    hasData = GetFilenameTimestamp(Path.GetFileName(path), out dateTaken, ref src);
                    break;

                case ".mp4":
                    hasData = GetMp4Date(path, out dateTaken, out src);
                    break;

                case ".mkv":
                    hasData = GetFilenameTimestamp(Path.GetFileName(path), out dateTaken, ref src);
                    break;
            }

            return (hasData, src);
        }

        //Try and examine JPG metadata. If necessary, it analyzes filename. If can't find data in either, default to DateTime.Now.
        private static bool GetJpgDate(string path, out DateTime dateTaken, ref DateTakenSrc src)
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
                hasData = GetFilenameTimestamp(Path.GetFileName(path), out dateTaken, ref src);
                if (!hasData) dateTaken = DateTime.Now;
            }

            return hasData;
        }

        ///<summary>Get when an mp4 file was taken.</summary>
        ///<returns>True if this file had data.</returns>
        private static bool GetMp4Date(string path, out DateTime dateTaken, out DateTakenSrc src)
        {
            dateTaken = DateTime.Now;
            src = DateTakenSrc.Now;
            
            try
            {
                IEnumerable<MetadataExtractor.Directory> directories = QuickTimeMetadataReader.ReadMetadata(new FileStream(path, FileMode.Open));
                QuickTimeMovieHeaderDirectory? directory = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();

                if (directory != null && directory.TryGetDateTime(QuickTimeMovieHeaderDirectory.TagCreated, out dateTaken))
                {
                    src = DateTakenSrc.Metadata;
                    return true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                src = DateTakenSrc.Now;
                bool hasData = GetFilenameTimestamp(Path.GetFileName(path), out dateTaken, ref src);
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
        private static bool GetFilenameTimestamp(string filename, out DateTime dateTaken, ref DateTakenSrc src)
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
                    hasData = ParseTimestamp(timestamp, out dateTaken, ref src);
            }

            return hasData;
        }

        //Try parsing timestamp like this: "20211031155822"
        //Returns false if unable to parse.
        private static bool ParseTimestamp(string timestamp, out DateTime dateTime, ref DateTakenSrc src)
        {
            if (timestamp.Length < 14 || DateTime.TryParse(timestamp, out dateTime))
            {
                dateTime = DateTime.Now;
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