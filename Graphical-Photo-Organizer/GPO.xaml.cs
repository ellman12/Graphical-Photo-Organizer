using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
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

    ///Full path to Unknown Date Taken folder.
    private string unknownDTFolderPath = "";
    
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

    ///Begin the sorting process.
    private void beginBtn_Click(object sender, RoutedEventArgs e)
    {
        itemPreview.LoadedBehavior = MediaState.Play;
        itemPreview.Visibility = Visibility.Visible;
        setupGroupBox.IsEnabled = false;
        currentItemGroupBox.IsEnabled = true;
        muteUnmuteBtn.IsEnabled = true;
        unknownDTFolderPath = Path.Combine(destDirRootPath, "Unknown Date Taken").Replace('\\', '/');

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
        destPathLabel.Content = destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
    }

    ///Set value in warning label. Pass in "" or null to clear warning labels.
    private void SetWarning(string? newText)
    {
        warningLabel.Content = String.IsNullOrWhiteSpace(newText) ? null : "Warning";
        warningTextLabel.Content = newText;
    }
    
    ///Leaves the current item where it is and loads the next item.
    private void SkipBtn_Click(object sender, RoutedEventArgs e)
    {
        amountSkipped++;
        UpdateStats();
        
        if (unsortedFiles.Count > 0) LoadItem();
        else if (unsortedFiles.Count == 0) Cleanup();
    }
    
    ///Undoes any modifications made to the file by the user. The filename and Date Taken are set to what they were when file first loaded.
    private void resetBtn_Click(object sender, RoutedEventArgs e)
    {
        filenameTextBox.Text = filename;
        newDateTakenLabel.Content = "New: " + dateTaken.ToString("M/d/yyyy", CultureInfo.InvariantCulture);
        datePicker.DisplayDate = dateTaken;
        datePicker.SelectedDate = dateTaken;
    }

    ///Deletes the current item, and, if enabled, prompts the user to confirm recycling of the item.
    private void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (delWarnCheckBox.IsChecked == true)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Delete this item?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
        }
        
        RecycleFile(currItemFullPath);
        LoadItem();
        amountDeleted++;
        UpdateStats();
    }

    ///Moves the current item to its new home and loads the next item.
    private void nextItemBtn_Click(object sender, EventArgs e)
    {
        // UpdateDestPath(); //TODO: don't think update dest path needed here.
        MoveItem(false);
    }
    
    ///Sends the current item to the Unknown Date Taken folder and loads the next item.
    private void UnknownDateBtn_OnClick(object sender, RoutedEventArgs e)
    {
        //TODO: when eventually make DT field nullable, put this in UpdateDestPath() and have it check if it is null. If it is make this the dest path, else use the DT for the path.
        destPathLabel.Content = destFilePath = Path.Combine(unknownDTFolderPath, filenameTextBox.Text + ext);
        MoveItem(true);
    }

    ///<summary>Called when either next button or unknown DT button clicked. Moves the current item to its new location (destFilePath).</summary>
    ///<param name="unknownDT">Pass in 'true' if this item will be sent to the Unknown Date Taken folder. False if it will be sent to a sorted folder (determined in UpdateDestPath()).</param>
    private void MoveItem(bool unknownDT)
    {
        //If there is an item with the exact same full path, ask user what to do. They can overwrite it, skip it, or cancel.
        if (File.Exists(destFilePath))
        {
            MessageBoxResult result = MessageBox.Show("A file with the same name already exists at that location. Overwrite it with this file?\nYes will overwrite it with this file, No will keep the original file and move on to the next file to sort, Cancel will cancel this.", "File already exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Error);

            if (result == MessageBoxResult.Yes) RecycleFile(destFilePath); //Delete the original
            else if (result == MessageBoxResult.Cancel) return; //Abort the move process
        }
        else //Move like normal
        {
            Directory.CreateDirectory(unknownDT ? unknownDTFolderPath : destFolderPath);

            //Stupid but fixes file in use error
            // GC.Collect(); TODO: needed?
            // GC.WaitForPendingFinalizers();
            
            //Only needed here because if an item with the same exact path already existed, no need to re-add.
            destDirContents.Add(destFilePath.Replace(unknownDT ? unknownDTFolderPath : destFolderPath, null), filenameTextBox.Text);
        }
        new Thread(() => File.Move(currItemFullPath, destFilePath)).Start();
        
        if (unsortedFiles.Count > 0) LoadItem();
        else if (unsortedFiles.Count == 0) Cleanup();
    }

    ///<summary>Runs garbage collection and recycles the file specified</summary>
    private static void RecycleFile(string path)
    {
        // GC.Collect(); TODO: needed?
        // GC.WaitForPendingFinalizers();
        new Thread(() => FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin)).Start(); //https://stackoverflow.com/a/3282456
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