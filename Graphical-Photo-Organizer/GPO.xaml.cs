using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualBasic.FileIO;
using MessageBox = System.Windows.MessageBox;
using WinForms = System.Windows.Forms;
using M = Graphical_Photo_Organizer.Metadata;

namespace Graphical_Photo_Organizer;

///<summary>Interaction logic for GPO.xaml</summary>
public partial class GPO
{
    //Set once during setup by user.
    ///Stores every short path (relative to destDirRootPath) and its filename in the directory where sorted items end up. Populated with all the items, if any, in destDirRootPath. When an item is sorted, it's added to this.
    private readonly Dictionary<string, string> destDirContents = new();

    ///Every full path in the unsorted folder needing sorting.
    private Queue<string> unsortedFiles = new();
    
    ///Full path of folder to sort and full path of where to send sorted items.
    private string srcDirRootPath = "", destDirRootPath = "";

    ///The current item's full original path.
    private string currItemFullPath = "";

    //Set when current file loads and stays constant until next file loaded or finish sorting.
    ///Current file's extension. This is not included in the filename TextBox but is appended when the file is moving.
    private string ext = "";
    
    ///Where the DT GPO found came from.
    private M.DateTakenSrc dateTakenSrc;

    //Set when current file loads and may change based on user's actions.
    ///What the filename was when file loaded, and can user can change.
    private string filename = "";
    
    ///Full path to folder where the current item will be sent.
    private string destFolderPath = "";
    
    ///The full final path of current item (destFolderPath + filename).
    private string destFilePath = "";
    
    ///The Date Taken that was found (or not) when first loaded.
    private DateTime dateTaken;

    //Stats that are updated automatically as user sorts the folder.
    private int amountSorted;
    private int amountSkipped;
    private int amountDeleted;

    public GPO() => InitializeComponent();

    private void Window_Initialized(object sender, EventArgs e)
    {
        //These are necessary.
        datePicker.DisplayDate = DateTime.Now;
        datePicker.SelectedDate = DateTime.Now;
        
        srcDirLabel.Content = "";
        destDirLabel.Content = "";
        originalPathLabel.Content = "";
        destPathLabel.Content = "";
        statsLabel.Content = "";
        dateTakenSrcLabel.Content = "";
        warningLabel.Content = "";
        warningTextLabel.Content = "";

        //Debugging stuff
        // srcDirLabel.Content = srcDirRootPath = "C:/Users/Elliott/Videos/Photos-001";
        // destDirLabel.Content = destDirRootPath = "C:/Users/Elliott/Videos/sorted";
        // unsortedFiles = GetSupportedFiles(srcDirRootPath);
        // ValidateFolderDirs();
    }

    ///<summary>Used for getting the source and destination folders. Uses a WinForms folder browser dialog.</summary>
    ///<param name="winTitle">What the folder browser dialog should display in the window title.</param>
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
    
    ///<summary>
    ///<para>Get the full paths of all supported file types in a root path.</para>
    ///Supported file types are: ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mkv", ".mov"
    ///</summary>
    private static Queue<string> GetSupportedFiles(string rootPath)
    {
        string[] validExts = {".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mkv", ".mov"};
        string[] allPaths = Directory.GetFiles(rootPath, "*.*", System.IO.SearchOption.AllDirectories);
        Queue<string> goodPaths = new();
        
        foreach (string path in allPaths)
        {
            if (validExts.Contains(Path.GetExtension(path)))
                goodPaths.Enqueue(path.Replace('\\', '/'));
        }

        return goodPaths;
    }

    private void chooseSrcBtn_Click(object sender, RoutedEventArgs e)
    {
        string path = SelectFolder("Select Folder of Images to Sort");
        if (path == "") return;

        unsortedFiles.Clear(); //Clear if user changed to different src folder
        unsortedFiles = GetSupportedFiles(path);

        srcDirRootPath = path;
        srcDirLabel.ToolTip = srcDirRootPath.Replace('\\', '/');
        srcDirLabel.Content = srcDirRootPath.Replace('\\', '/');
        ValidateFolderDirs();
    }

    private void chooseDestBtn_Click(object sender, RoutedEventArgs e)
    {
        string path = SelectFolder("Select Root Folder Where Sorted Items Will Go");
        if (path == "") return;

        destDirRootPath = path;
        destDirLabel.Content = destDirRootPath.Replace('\\', '/');
        destDirLabel.ToolTip = destDirRootPath.Replace('\\', '/');

        ValidateFolderDirs();
    }

    private void beginBtn_Click(object sender, RoutedEventArgs e)
    {
        itemPreview.LoadedBehavior = MediaState.Play;
        itemPreview.Visibility = Visibility.Visible;
        setupGroupBox.IsEnabled = false;
        currentItemGroupBox.IsEnabled = true;
        muteUnmuteBtn.IsEnabled = true;

        //Add any filenames in the destination folder for dupe checking.
        foreach(string fullPath in GetSupportedFiles(destDirRootPath))
            destDirContents.Add(fullPath.Replace(destDirRootPath, null).Replace('\\', '/'), Path.GetFileName(fullPath));
        LoadItem();
        UpdateStats();
    }

    ///<summary>Dequeue the full path at the front and populate the GUI controls with values.</summary>
    private void LoadItem()
    {
        originalPathLabel.Content = currItemFullPath = unsortedFiles.Dequeue(); //The item to load.
        filenameTextBox.Text = filename = Path.GetFileNameWithoutExtension(currItemFullPath);
        ext = Path.GetExtension(currItemFullPath);
        itemPreview.Source = new Uri(currItemFullPath);

        M.GetDateTaken(currItemFullPath, out dateTaken, out dateTakenSrc);
        ogDateTakenLabel.Content = "OG: " + dateTaken.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
        datePicker.SelectedDate = datePicker.DisplayDate = dateTaken;

        dateTakenSrcLabel.Content = dateTakenSrc;
        dateTakenSrcLabel.Foreground = dateTakenSrc switch
        {
            M.DateTakenSrc.Metadata => Brushes.Green,
            M.DateTakenSrc.Filename => Brushes.Goldenrod,
            M.DateTakenSrc.Now => Brushes.Red,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        UpdateDestPath();
        
        //Checks the destination folder to see if the current item is/might be a duplicate.
        string destFilename = Path.GetFileName(destFilePath);
        SetWarning(destDirContents.ContainsValue(destFilename) ? $"A file with the same name already exists at {destDirContents.First(x => x.Value == destFilename).Key}" : null);
    }

    ///<summary>Updates the folder where the current photo will be sent and also its final path and the label that displays the full path.</summary>
    private void UpdateDestPath()
    {
        destFolderPath = Path.Combine(destDirRootPath, datePicker.SelectedDate?.ToString("yyyy/M MMMM/d", CultureInfo.InvariantCulture)!).Replace('\\', '/');
        destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
        destPathLabel.Content = destFilePath;
    }

    ///Set value in warning label. Pass in "" or null to clear warning labels.
    private void SetWarning(string? newText)
    {
        warningLabel.Content = String.IsNullOrWhiteSpace(newText) ? null : "Warning";
        warningTextLabel.Content = newText;
    }

    private void SkipBtn_Click(object sender, RoutedEventArgs e)
    {
        // unsortedFiles.Dequeue(); TODO: might not be needed
        if (unsortedFiles.Count > 0) LoadItem();
        else if (unsortedFiles.Count == 0) Cleanup();

        amountSkipped++;
        UpdateStats();
    }

    ///<summary>Moves the current item to its new home and loads the next item.</summary>
    private async void nextItemBtn_Click(object sender, EventArgs e)
    {
        // UpdateDestPath(); //TODO: don't think update dest path needed here.

        //If there is an item with the exact same full path, ask user what to do. They can overwrite it, skip it, or cancel.
        if (File.Exists(destFilePath))
        {
            MessageBoxResult result = MessageBox.Show("A file with the same name already exists at that location. Overwrite it with this file?\nYes will overwrite it with this file, No will keep the original file and move on to the next file to sort, Cancel will cancel this.", "File already exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes) RecycleFile(destFilePath); //Delete the original
            else if (result == MessageBoxResult.Cancel) return; //Abort the move process
        }
        else //Move like normal.
        {
            Directory.CreateDirectory(destFolderPath);

            //Stupid but fixes file in use error
            // GC.Collect(); TODO: needed?
            // GC.WaitForPendingFinalizers();
            
            //Only needed here because if an item with the same exact path already existed, no need to re-add.
            destDirContents.Add(destFilePath.Replace(destDirRootPath, null), filenameTextBox.Text);
        }
        await Task.Run(() => File.Move(currItemFullPath, destFilePath));
        
        if (unsortedFiles.Count > 0) LoadItem();
        else if (unsortedFiles.Count == 0) Cleanup();
    }
    
    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (delWarnCheckBox.IsChecked == false)
            Recycle();
        else if (delWarnCheckBox.IsChecked == true)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Delete this item?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                Recycle();
        }

        void Recycle()
        {
            string deletePath = unsortedFiles[0];
            unsortedFiles.RemoveAt(0);
            amountDeleted++;
            UpdateStats();

            if (unsortedFiles.Count > 0)
                LoadItem(unsortedFiles[0]);
            else
                Cleanup();

            RecycleFile(deletePath);
        }
    }

    ///<summary>Removes the current image from the List and loads the next one.</summary>
    private void ReplaceMeLol()
    {
        unsortedFiles.RemoveAt(0);
        amountSorted++;
        UpdateStats();

        if (unsortedFiles.Count > 0)
            LoadItem(unsortedFiles[0]);
    }

    ///<summary>Runs garbage collection and recycles the file specified</summary>
    private static void RecycleFile(string path)
    {
        // GC.Collect(); TODO: needed?
        // GC.WaitForPendingFinalizers();
        new System.Threading.Thread(() => FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin)).Start(); //https://stackoverflow.com/a/3282456
    }

    private void resetBtn_Click(object sender, RoutedEventArgs e)
    {
        filenameTextBox.Text = filename;
        newDateTakenLabel.Content = "New: " + dateTaken.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
        datePicker.DisplayDate = dateTaken;
        datePicker.SelectedDate = dateTaken;
    }

    private void OriginalPathLabel_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //https://stackoverflow.com/a/1132559
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = Path.GetDirectoryName(unsortedFiles[0]),
            UseShellExecute = true,
            Verb = "open"
        });
    }

    private async void UnknownDateBtn_OnClick(object sender, RoutedEventArgs e)
    {
        destFolderPath = Path.Combine(destDirRootPath, "Unknown Date Taken").Replace('\\', '/');
        destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
        destPathLabel.Content = destFilePath;
        
        if (!File.Exists(destFilePath))
        {
            Directory.CreateDirectory(destFolderPath);

            //Stupid but fixes file in use error
            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (unsortedFiles.Count >= 2)
                itemPreview.Source = new Uri(unsortedFiles[1]);
            else if (unsortedFiles.Count == 1) //Since the Source can't be set to "" for whatever reason, just hide the control when all items are sorted.
                Cleanup();

            await Task.Run(() => File.Move(unsortedFiles[0], destFilePath));
            destDirContents.Add(destFilePath.Replace(destDirRootPath, null), filenameTextBox.Text);
        }
        else
        {
            MessageBoxResult result = MessageBox.Show("A file with the same name already exists at that location. Overwrite it with this file?\nYes will overwrite it with this file, No will keep the original file and move on to the next file to sort, Cancel will cancel this.", "File already exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes)
            {
                RecycleFile(destFilePath); //Delete the original
                await Task.Run(() => File.Move(unsortedFiles[0], destFilePath)); //And replace with this one. Don't need to add to Dict because it already has this in it from before.
            }
            else if (result == MessageBoxResult.No)
            {
                ReplaceMeLol();
                return;
            }
            else if (result == MessageBoxResult.Cancel)
                return;
        }

        ReplaceMeLol();

        if (unsortedFiles.Count == 0)
            Cleanup();
    }
    
    private void Cleanup()
    {
        itemPreview.LoadedBehavior = MediaState.Manual;
        itemPreview.Visibility = Visibility.Hidden;
        itemPreview.Stop();
        
        // unsortedFiles.Clear(); probs don't need
        //TODO: uncomment later
        // srcDirRootPath = destDirRootPath = ext = "";
        // dateTakenSrc = M.DateTakenSrc.Now;
        // filename = destFolderPath = destFilePath = "";
        // dateTaken = DateTime.Now;
        // amountSorted = 0;
        // amountSkipped = 0;
        // amountDeleted = 0;

        filenameTextBox.Text = "";
        originalPathLabel.Content = "";
        destPathLabel.Content = "";
        ogDateTakenLabel.Content = "";
        newDateTakenLabel.Content = "";
        dateTakenSrcLabel.Content = "";
        currentItemGroupBox.IsEnabled = false;
        setupGroupBox.IsEnabled = true;
        srcDirLabel.Content = srcDirRootPath = "";
        destDirLabel.Content = destDirRootPath = "";
        muteUnmuteBtn.IsEnabled = false;
        SetWarning(null);
        destDirContents.Clear();
    }

    private void UpdateStats() => statsLabel.Content = $"{amountSorted} Sorted   {amountSkipped} Skipped   {amountDeleted} Deleted   {unsortedFiles.Count} Left";

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
}