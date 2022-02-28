using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using WinForms = System.Windows.Forms;
using M = Graphical_Photo_Organizer.Metadata;

namespace Graphical_Photo_Organizer
{
    /// <summary>
    /// Interaction logic for GPO.xaml
    /// </summary>
    public partial class GPO : Window
    {
        //Set during setup
        private List<string> unsortedFiles = new();
        private string srcDirRootPath = "", destDirRootPath = "";

        //Set on file load and stays constant
        private string ext = "";
        private M.DateTakenSrc dateTakenSrc;

        //Set on load and can be changed by user //TODO is this accurate?
        private string ogFilename = "";
        private string destFolderPath = ""; //Folder where the current photo will end
        private string destFilePath = ""; //The full final path to it.
        private DateTime ogDateTaken;

        //Stats
        private int amountSorted = 0;
        private int amountSkipped = 0;
        private int amountDeleted = 0;

        public GPO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Used for getting the source and destination folders. Uses a WinForms folder browser dialog.
        /// </summary>
        /// <param name="winTitle">What the folder browser dialog should display</param>
        /// <returns>The selected folder path.</returns>
        private static string SelectFolder(string winTitle)
        {
            using var dialog = new WinForms.FolderBrowserDialog();
            dialog.UseDescriptionForTitle = true;
            dialog.Description = winTitle;
            DialogResult result = dialog.ShowDialog();

            if (result == WinForms.DialogResult.OK && !String.IsNullOrWhiteSpace(dialog.SelectedPath))
                return dialog.SelectedPath;
            else
                return "";
        }

        ///<summary>Checks the folder paths the user chose to make sure they are valid and they can proceed to the sorting.</summary>
        private void ValidateFolderDirs()
        {
            if (unsortedFiles.Count == 0)
            {
                MessageBox.Show("There were no supported files found in the source folder provided.", "No supported files found in folder", MessageBoxButton.OK, MessageBoxImage.Error);
                beginBtn.IsEnabled = false;
                srcDirRootPath = "";
                srcDirLabel.Content = "";
            }
            else if (srcDirRootPath == destDirRootPath)
            {
                MessageBox.Show("You cannot have the same source and destination folder path!", "Same folder path specified", MessageBoxButton.OK, MessageBoxImage.Error);
                beginBtn.IsEnabled = false;
                srcDirRootPath = "";
                srcDirLabel.Content = "";
                destDirRootPath = "";
                destDirLabel.Content = "";
            }
            else if (!String.IsNullOrWhiteSpace(srcDirRootPath) && !String.IsNullOrWhiteSpace(destDirRootPath))
            {
                beginBtn.IsEnabled = true;
            }
            else
            {
                currentItemGroupBox.IsEnabled = false;
                beginBtn.IsEnabled = false;
            }
        }

        private void chooseSrcBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = SelectFolder("Select Folder of Images to Sort");

            //unsortedFiles.Clear(); //Clear if user changed to different src folder
            unsortedFiles = Directory.GetFiles(path, "*.jp*g", SearchOption.AllDirectories).ToList();
            unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.png", SearchOption.AllDirectories)).ToList();
            unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.gif", SearchOption.AllDirectories)).ToList();
            unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.mp4", SearchOption.AllDirectories)).ToList();
            unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.mkv", SearchOption.AllDirectories)).ToList();

            srcDirRootPath = path;
            srcDirLabel.Content = srcDirRootPath;
            ValidateFolderDirs();   
        }

        private void chooseDestBtn_Click(object sender, RoutedEventArgs e)
        {
            string path = SelectFolder("Select Root Folder Where Sorted Items Will Go");

            destDirRootPath = path;
            destDirLabel.Content = destDirRootPath;

            ValidateFolderDirs();
        }

        private void beginBtn_Click(object sender, RoutedEventArgs e)
        {
            currentItemGroupBox.IsEnabled = true;
            LoadItem(unsortedFiles[0]);
            setupGroupBox.IsEnabled = false;
            UpdateStats();
        }

        private void LoadItem(string path)
        {
            originalPathLabel.Content = unsortedFiles[0];
            ogFilename = Path.GetFileNameWithoutExtension(path);
            ext = Path.GetExtension(path);

            (bool hasData, dateTakenSrc) = M.GetDateTaken(path, out ogDateTaken);

            if (!hasData)
            {
                ogDateTaken = DateTime.Now;
                dateTakenSrc = M.DateTakenSrc.Now;
            }

            filenameTextBox.Text = ogFilename;
            ogDateTakenLabel.Content = ogDateTaken.ToString("M/d/yyyy");
            dateTakenSrcLabel.Content = "Source: " + dateTakenSrc;
            datePicker.SelectedDate = ogDateTaken;
            itemPreview.Source = new Uri(path);

            UpdateDestPath();
            CheckForDuplicates(path);
        }
        
        ///<summary>Updates the folder where the current photo will be sent and also its final path and the label that displays the full path.</summary>
        private void UpdateDestPath()
        {
            destFolderPath = Path.Combine(srcDirRootPath, datePicker.SelectedDate?.ToString("yyyy/M MMMM/d")!).Replace('\\', '/');
            destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
            destPathLabel.Content = destFilePath;
        }
        
        private void CheckForDuplicates(string path)
        {
            string[] sortedFiles = Directory.GetFiles(srcDirRootPath, "*.jp*g", SearchOption.AllDirectories);
            sortedFiles = sortedFiles.Concat(Directory.GetFiles(srcDirRootPath, "*.png", SearchOption.AllDirectories)).ToArray();

            foreach (string file in sortedFiles)
            {
                if (file.EndsWith(Path.GetFileName(path)))
                    MessageBox.Show("The current file might already be in the sorted folder at the path\n " + file.Replace('\\', '/') + "\nA file with the same name already is in the sorted folder.", "Possible Duplicate", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        private void UpdateStats() => statsLabel.Content = $"Amount Sorted: {amountSorted}    Amount Skipped: {amountSkipped}    Amount Deleted: {amountDeleted}    Amount Left: {unsortedFiles.Count}";
        
        private void filenameTextBox_TextChanged(object sender, EventArgs e) => UpdateDestPath();

        private void DatePicker_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            newDateTakenLabel.Content = datePicker.SelectedDate?.ToString("M/d/yyyy");
            UpdateDestPath();
        }
        
        /// <summary>Moves the current item to its new home and loads the next item.</summary>
        private void nextItemBtn_Click(object sender, EventArgs e)
        {
            UpdateDestPath();
            if (!File.Exists(destFilePath))
            {
                Directory.CreateDirectory(destFolderPath);

                try
                {
                    File.Move(unsortedFiles[0], destFilePath);
                }
                catch (IOException) //Stupid but fixes file in use error
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Move(unsortedFiles[0], destFilePath);
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("A file with the same name already exists at that location. Overwrite it with this file?\nYes will overwrite it with this file, No will keep the original file and move on to the next file to sort, Cancel will cancel this.", "File already exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(destFilePath);
                    File.Move(unsortedFiles[0], destFilePath);
                }
                else if (result == MessageBoxResult.No)
                {
                    LoadNextItem();
                    return;
                }
                else if (result == MessageBoxResult.Cancel)
                    return;
            }
        
            LoadNextItem();
        }

        ///<summary>Removes the current image from the List and loads the next one.</summary>
        private void LoadNextItem()
        {
            unsortedFiles.RemoveAt(0);
            amountSorted++;
            UpdateStats();
        
            if (unsortedFiles.Count > 0)
                LoadItem(unsortedFiles[0]);
        }
    }
}
