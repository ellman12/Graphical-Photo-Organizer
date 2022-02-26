using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
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
        private string unsortedDirRootPath = "", sortedDirRootPath = "";
        
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new WinForms.FolderBrowserDialog();
            dialog.UseDescriptionForTitle = true;
            dialog.Description = "Select Folder of Images to Sort";
            WinForms.DialogResult result = dialog.ShowDialog();

            if (result == WinForms.DialogResult.OK && !String.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                unsortedFiles = Directory.GetFiles(dialog.SelectedPath, "*.jp*g", SearchOption.AllDirectories).ToList();
                unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(dialog.SelectedPath, "*.png", SearchOption.AllDirectories)).ToList();
                unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(dialog.SelectedPath, "*.gif", SearchOption.AllDirectories)).ToList();
                unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(dialog.SelectedPath, "*.mp4", SearchOption.AllDirectories)).ToList();
                unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(dialog.SelectedPath, "*.mkv", SearchOption.AllDirectories)).ToList();

                unsortedDirRootPath = dialog.SelectedPath;
                // srcDirLabel.Text = unsortedDirRootPath; //TODO 
            }
            else if (result != WinForms.DialogResult.Cancel)
                MessageBox.Show("Unknown error when choosing folder. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
