using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;
using WinForms = System.Windows.Forms;
using M = Graphical_Photo_Organizer.Metadata;

namespace Graphical_Photo_Organizer;

///<summary>
///Interaction logic for GPO.xaml
///</summary>
public partial class GPO
{
    private static readonly string[] videoFileExts = {".mp4", ".mkv", ".mov"};

    //Set during setup
    private List<string> unsortedFiles = new();
    private string srcDirRootPath = "", destDirRootPath = "";

    //Set on file load and stays constant
    private string ext = "";
    private M.DateTakenSrc dateTakenSrc;

    //Set on load and can be changed by user
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

    private void Window_Initialized(object sender, EventArgs e)
    {
        datePicker.DisplayDate = DateTime.Now;
        datePicker.SelectedDate = DateTime.Now;
        srcDirLabel.Content = "";
        destDirLabel.Content = "";
        originalPathLabel.Content = "";
        destPathLabel.Content = "";
        statsLabel.Content = "";
        dateTakenSrcLabel.Content = "";
        muteUnmuteBtn.Visibility = Visibility.Hidden;
    }

    ///<summary>
    ///Used for getting the source and destination folders. Uses a WinForms folder browser dialog.
    ///</summary>
    ///<param name="winTitle">What the folder browser dialog should display</param>
    ///<returns>The selected folder path.</returns>
    private static string SelectFolder(string winTitle)
    {
        using var dialog = new FolderBrowserDialog();
        dialog.UseDescriptionForTitle = true;
        dialog.Description = winTitle;
        DialogResult result = dialog.ShowDialog();

        if (result == WinForms.DialogResult.OK && !String.IsNullOrWhiteSpace(dialog.SelectedPath))
            return dialog.SelectedPath;
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

        if (path == "") return;

        unsortedFiles.Clear(); //Clear if user changed to different src folder
        unsortedFiles = Directory.GetFiles(path, "*.jp*g", SearchOption.AllDirectories).ToList();
        unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.png", SearchOption.AllDirectories)).ToList();
        unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.gif", SearchOption.AllDirectories)).ToList();
        unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.mp4", SearchOption.AllDirectories)).ToList();
        unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.mkv", SearchOption.AllDirectories)).ToList();
        unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(path, "*.mov", SearchOption.AllDirectories)).ToList();

        srcDirRootPath = path;
        srcDirLabel.ToolTip = srcDirRootPath.Replace('\\', '/');
        srcDirLabel.Content = srcDirRootPath.Replace('\\', '/');
        ValidateFolderDirs();
    }

    private void chooseDestBtn_Click(object sender, RoutedEventArgs e)
    {
        string path = SelectFolder("Select Root Folder Where Sorted Items Will Go");

        destDirRootPath = path;
        destDirLabel.Content = destDirRootPath.Replace('\\', '/');
        destDirLabel.ToolTip = destDirRootPath.Replace('\\', '/');

        ValidateFolderDirs();
    }

    private async void beginBtn_Click(object sender, RoutedEventArgs e)
    {
        EnableItemPreview();
        setupGroupBox.IsEnabled = false;
        currentItemGroupBox.IsEnabled = true;
        await LoadItem(unsortedFiles[0]);
        UpdateStats();
        UpdateMuteBtn();
    }

    private async Task LoadItem(string path)
    {
        originalPathLabel.Content = unsortedFiles[0].Replace('\\', '/');
        ogFilename = Path.GetFileNameWithoutExtension(path);
        ext = Path.GetExtension(path);

        (bool hasData, dateTakenSrc) = M.GetDateTaken(path, out ogDateTaken);

        if (!hasData)
        {
            ogDateTaken = DateTime.Now;
            dateTakenSrc = M.DateTakenSrc.Now;
        }

        filenameTextBox.Text = ogFilename;
        ogDateTakenLabel.Content = "OG: " + ogDateTaken.ToString("M/d/yyyy", CultureInfo.InvariantCulture);

        dateTakenSrcLabel.Content = dateTakenSrc;
        dateTakenSrcLabel.Foreground = dateTakenSrc switch
        {
            M.DateTakenSrc.Metadata => Brushes.Green,
            M.DateTakenSrc.Filename => Brushes.Yellow,
            M.DateTakenSrc.Now => Brushes.Red,
            _ => throw new ArgumentOutOfRangeException()
        };

        datePicker.DisplayDate = ogDateTaken;
        datePicker.SelectedDate = ogDateTaken;
        itemPreview.Source = new Uri(path);

        UpdateDestPath();
        await Task.Run(CheckForDuplicates);
    }

    ///<summary>Updates the folder where the current photo will be sent and also its final path and the label that displays the full path.</summary>
    private void UpdateDestPath()
    {
        destFolderPath = Path.Combine(destDirRootPath, datePicker.SelectedDate?.ToString("yyyy/M MMMM/d", CultureInfo.InvariantCulture)!).Replace('\\', '/');
        destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
        destPathLabel.Content = destFilePath;
    }

    ///<summary>Checks the destination folder for items that either might be or are duplicates.</summary>
    private void CheckForDuplicates()
    {
        string[] sortedFiles = Directory.GetFiles(destDirRootPath, "*.jp*g", SearchOption.AllDirectories).ToArray();
        sortedFiles = sortedFiles.Concat(Directory.GetFiles(destDirRootPath, "*.png", SearchOption.AllDirectories)).ToArray();
        sortedFiles = sortedFiles.Concat(Directory.GetFiles(destDirRootPath, "*.gif", SearchOption.AllDirectories)).ToArray();
        sortedFiles = sortedFiles.Concat(Directory.GetFiles(destDirRootPath, "*.mp4", SearchOption.AllDirectories)).ToArray();
        sortedFiles = sortedFiles.Concat(Directory.GetFiles(destDirRootPath, "*.mkv", SearchOption.AllDirectories)).ToArray();
        sortedFiles = sortedFiles.Concat(Directory.GetFiles(destDirRootPath, "*.mov", SearchOption.AllDirectories)).ToArray();

        foreach (string file in sortedFiles)
        {
            if (file.EndsWith(Path.GetFileName(destFilePath)))
                MessageBox.Show("The current file might already be in the sorted folder at the path\n " + file.Replace('\\', '/') + "\nA file with the same name already is in the sorted folder.", "Possible Duplicate", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void UpdateStats() => statsLabel.Content = $"Amount Sorted: {amountSorted}    Amount Skipped: {amountSkipped}    Amount Deleted: {amountDeleted}    Amount Left: {unsortedFiles.Count}";

    private void UpdateMuteBtn() => muteUnmuteBtn.Visibility = videoFileExts.Contains(ext) ? Visibility.Visible : Visibility.Hidden;

    private void MuteUnmuteBtn_Click(object sender, RoutedEventArgs e)
    {
        itemPreview.IsMuted = !itemPreview.IsMuted;
        muteUnmuteBtn.Content = itemPreview.IsMuted ? "Un_mute" : "_Mute";
    }

    private void filenameTextBox_TextChanged(object sender, EventArgs e) => UpdateDestPath();

    private void DatePicker_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        newDateTakenLabel.Content = "New: " + datePicker.SelectedDate?.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
        UpdateDestPath();
        nextItemBtn.Focus();
    }

    ///<summary>If nothing left to sort, clear the item preview</summary>
    private void ClearItemPreview()
    {
        itemPreview.LoadedBehavior = MediaState.Manual;
        itemPreview.Visibility = Visibility.Hidden;
        itemPreview.IsMuted = true;
        itemPreview.Stop();

        filenameTextBox.Text = "";
        originalPathLabel.Content = "";
        destPathLabel.Content = "";
        ogDateTakenLabel.Content = "";
        newDateTakenLabel.Content = "";
        dateTakenSrcLabel.Content = "";
    }

    ///<summary>If starting another round of sorting after finishing one, re-enable and show the item preview.</summary>
    private void EnableItemPreview()
    {
        itemPreview.LoadedBehavior = MediaState.Play;
        itemPreview.Visibility = Visibility.Visible;
    }

    private async void SkipBtn_Click(object sender, RoutedEventArgs e)
    {
        unsortedFiles.RemoveAt(0);

        if (unsortedFiles.Count > 0)
            await LoadItem(unsortedFiles[0]);
        else if (unsortedFiles.Count == 0)
            ClearItemPreview();

        amountSkipped++;
        UpdateStats();
        UpdateMuteBtn();
    }

    ///<summary>Moves the current item to its new home and loads the next item.</summary>
    private async void nextItemBtn_Click(object sender, EventArgs e)
    {
        UpdateDestPath();
        if (!File.Exists(destFilePath))
        {
            Directory.CreateDirectory(destFolderPath);

            //Stupid but fixes file in use error
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (unsortedFiles.Count >= 2)
                itemPreview.Source = new Uri(unsortedFiles[1]);
            else if (unsortedFiles.Count == 1) //Since the Source can't be set to "" for whatever reason, just hide the control when all items are sorted.
                ClearItemPreview();

            await Task.Run(() => File.Move(unsortedFiles[0], destFilePath));
        }
        else
        {
            MessageBoxResult result = MessageBox.Show("A file with the same name already exists at that location. Overwrite it with this file?\nYes will overwrite it with this file, No will keep the original file and move on to the next file to sort, Cancel will cancel this.", "File already exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                File.Delete(destFilePath); //Delete the original
                await Task.Run(() => File.Move(unsortedFiles[0], destFilePath)); //And replace with this one
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

        if (unsortedFiles.Count == 0)
        {
            currentItemGroupBox.IsEnabled = false;
            setupGroupBox.IsEnabled = true;
            srcDirLabel.Content = srcDirRootPath = "";
            destDirLabel.Content = destDirRootPath = "";
        }
    }

    ///<summary>Removes the current image from the List and loads the next one.</summary>
    private async void LoadNextItem()
    {
        unsortedFiles.RemoveAt(0);
        amountSorted++;
        UpdateStats();
        UpdateMuteBtn();

        if (unsortedFiles.Count > 0)
            await LoadItem(unsortedFiles[0]);
    }

    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this photo?", "Delete this photo?", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            File.Delete(unsortedFiles[0]);
            unsortedFiles.RemoveAt(0);
            amountDeleted++;
            UpdateStats();
            UpdateMuteBtn();

            if (unsortedFiles.Count > 0)
                await LoadItem(unsortedFiles[0]);
            else
                ClearItemPreview();
        }
    }

    private void resetBtn_Click(object sender, RoutedEventArgs e)
    {
        filenameTextBox.Text = ogFilename;
        newDateTakenLabel.Content = "New: " + ogDateTaken.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
        datePicker.DisplayDate = ogDateTaken;
        datePicker.SelectedDate = ogDateTaken;
    }
}