using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using WinForms = System.Windows.Forms;
// using M = Graphical_Photo_Organizer.Metadata;

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
        // private M.DateTakenSrc dateTakenSrc;

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
    }
}
