using System;
using System.Collections.Generic;
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
using D = DateTakenExtractor.DateTakenExtractor;
using S = Graphical_Photo_Organizer.Shared;

namespace Graphical_Photo_Organizer;

///<summary>Interaction logic for GPO.xaml</summary>
public partial class GPO
{
    ///Represents the Settings window that can be shown/hidden whenever.
    private readonly Settings settings = new();
    
    //Set once during setup by user.
    ///Stores every filename in destDirRootPath, and the short path of it relative to destDirRootPath. Populated with all the items, if any, in destDirRootPath. When an item is sorted, it's added to this.
    private readonly Dictionary<string, string> destDirContents = new();

    ///Every full path in the unsorted folder needing sorting.
    private Queue<string> unsortedFiles = new();
    
    ///Full path of folder to sort and full path of where to send sorted items.
    private string srcDirRootPath = "", destDirRootPath = "";

    ///The current item's full original path.
    private string currItemFullPath = "";

    //Set when current file loads and stays constant until next file loaded or finish sorting.
    ///Current file's extension. This is not included in the ogFilename TextBox but is appended when the file is moving.
    private string ext = "";
    
    ///Where the DT GPO found came from.
    private D.DateTakenSrc dateTakenSrc;
    
    ///Used to verify filename user entered doesn't contain any invalid characters.
    private readonly HashSet<char> invalidChars = Path.GetInvalidFileNameChars().ToHashSet();

    //Set when current file loads and may change based on user's actions.
    ///What the ogFilename was when file loaded, and can user can change.
    private string ogFilename = "";
    
    ///Full path to folder where the current item will be sent.
    private string destFolderPath = "";

    ///Full path to Unknown Date Taken folder.
    private string unknownDTFolderPath = "";
    
    ///The full final path of current item (destFolderPath + ogFilename).
    private string destFilePath = "";
    
    ///The Date Taken that was found (or not) when first loaded.
    private DateTime? ogDateTaken;

    ///The new Date Taken that the user picked.
    private DateTime? newDateTaken;

    //Stats that are updated automatically as user sorts the folder.
    private int amountSorted;
    private int amountSkipped;
    private int amountDeleted;

    public GPO() => InitializeComponent();

    ///Closes both windows and closes the app.
    protected override void OnClosed(EventArgs e) //https://stackoverflow.com/a/9992888
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void Window_Initialized(object sender, EventArgs e)
    {
        datePicker.DisplayDate = DateTime.Now; //This is necessary to prevent null errors.
        datePicker.SelectedDate = null;
        timePicker.Value = null;
        newDateTakenLabel.Content = null;
        
        srcDirLabel.Content = ""; //These cannot be null because if they are, they don't take up any space and make UI look weird.
        destDirLabel.Content =""; 
        originalPathText.Text = null;
        destPathText.Text = null;
        statsLabel.Content = null;
        dateTakenSrcLabel.Content = null;
        warningText.Text = null;
        statusTextBlock.Text = null;

        settings.yearGtTB.Text = DateTime.Now.Year.ToString();
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
    ///Allowed file types are, depending on the settings of the user: "jpg", "jpeg", "png", "gif", "mp4", "mkv", "mov"
    ///</summary>
    private static Queue<string> GetSupportedFiles(string rootPath)
    {
        string[] allPaths = Directory.GetFiles(rootPath, "*.*", System.IO.SearchOption.AllDirectories);
        Queue<string> goodPaths = new();
        
        foreach (string path in allPaths)
        {
            if (S.allowedExts.Contains(Path.GetExtension(path).Replace(".", "").ToLower()))
                goodPaths.Enqueue(path.Replace('\\', '/'));
        }

        return goodPaths;
    }

    private void chooseSrcBtn_Click(object sender, RoutedEventArgs e)
    {
        string path = SelectFolder("Select Folder of Images to Sort");
        if (path == "") return;

        unsortedFiles = GetSupportedFiles(path);

        srcDirLabel.Content = srcDirLabel.ToolTip = srcDirRootPath = path.Replace('\\', '/');
        ValidateFolderDirs();
    }

    private void chooseDestBtn_Click(object sender, RoutedEventArgs e)
    {
        string path = SelectFolder("Select Root Folder Where Sorted Items Will Go");
        if (path == "") return;

        destDirLabel.Content = destDirLabel.ToolTip = destDirRootPath = path.Replace('\\', '/');
        ValidateFolderDirs();
    }
    
    ///Opens or closes the Settings window.
    private void SettingsBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (settings.IsVisible) settings.Hide();
        else settings.Show();
    }

    ///Begin the sorting process.
    private void beginBtn_Click(object sender, RoutedEventArgs e)
    {
	    progressBar.Value = amountSorted = amountSkipped = amountDeleted = 0;
        itemPreview.LoadedBehavior = MediaState.Play;
        itemPreview.Source = null;
        setupGroupBox.IsEnabled = false;
        unknownDTFolderPath = Path.Combine(destDirRootPath, "Unknown Date Taken").Replace('\\', '/');

        //Add any filenames in the destination folder for dupe checking.
        foreach(string fullPath in GetSupportedFiles(destDirRootPath))
            destDirContents.TryAdd(Path.GetFileName(fullPath), fullPath.Replace(destDirRootPath, null).Replace('\\', '/'));
        
        //EXPERIMENTAL
        // int amountMoved = 0;
        // const string expPath = "C:/Users/Elliott/Videos/exp";
        // Dictionary<string, string> srcDirContents = new();
        // foreach(string fullPath in GetSupportedFiles(srcDirRootPath))
        //     srcDirContents.Add(fullPath.Replace(srcDirRootPath, null).Replace('\\', '/'), Path.GetFileName(fullPath));
        //
        // foreach ((string key, string value) in srcDirContents)
        // {
        //     if (destDirContents.ContainsValue(value))
        //     {
        //         string ogPath = Path.Join(srcDirRootPath, key);
        //         string newPath = Path.Join(expPath, value);
        //         new System.Threading.Thread(() => File.Move(ogPath, newPath)).Start();
        //         amountMoved++;
        //     }
        // }
        //
        // if (amountMoved > 0) MessageBox.Show($"Moved {amountMoved} items", $"Moved {amountMoved} items", MessageBoxButton.OK, MessageBoxImage.Information);

        progressBar.Maximum = unsortedFiles.Count;
        
        if (settings.autoSortCheckBox.IsChecked == true)
        {
            if (settings.sendToUnknownDTBtn.IsChecked == true) new Thread(AutoSortSendToUnknownFolder).Start();
            else if (settings.promptBtn.IsChecked == true) new Thread(AutoSortPromptNullDT).Start();
            else if (settings.skipItemBtn.IsChecked == true) new Thread(AutoSortUnknownDTSkip).Start();
        }
        else //manual sorting
        {
	        currentItemGroupBox.IsEnabled = true;
			muteUnmuteBtn.IsEnabled = true;
			LoadAndDisplayNextItem();
		}
	}

    ///Dequeues the unsorted file at the start of the Queue and loads and displays it.
    private void LoadAndDisplayNextItem() => LoadAndDisplayItem(unsortedFiles.Dequeue());

    ///<summary>Load and display this item and populate GUI controls.</summary>
	///<param name="fullPath">The full path to the item to load and display.</param>
	private void LoadAndDisplayItem(string fullPath)
	{
		Dispatcher.Invoke(() =>
		{
			originalPathText.Text = currItemFullPath = fullPath;
			filenameTextBox.Text = ogFilename = Path.GetFileNameWithoutExtension(currItemFullPath);
			ext = Path.GetExtension(currItemFullPath);
			newDateTaken = ogDateTaken = D.GetDateTakenAuto(currItemFullPath, out dateTakenSrc);
			itemPreview.Source = new Uri(currItemFullPath);

			//Fixes file in use errors caused by video files.
			GC.Collect();
			GC.WaitForPendingFinalizers();

			if (ogDateTaken == null)
			{
				ogDateTakenLabel.Content = "None";
				newDateTakenLabel.Content = "None";
				timePicker.Value = null;
			}
			else if (ogDateTaken != null)
			{
				timePicker.Value = datePicker.SelectedDate = datePicker.DisplayDate = (DateTime) ogDateTaken;
				ogDateTakenLabel.Content = ogDateTaken?.ToString("M/d/yyyy h:mm:ss tt");
				newDateTakenLabel.Content = $"{newDateTaken?.ToString("M/d/yyyy")} {timePicker.Value?.ToString(" h:mm:ss tt")}";
			}

			dateTakenSrcLabel.Content = dateTakenSrc;
			dateTakenSrcLabel.Foreground = dateTakenSrc switch
			{
				D.DateTakenSrc.Metadata => Brushes.Green,
				D.DateTakenSrc.Filename => Brushes.Goldenrod,
				D.DateTakenSrc.None => Brushes.Red,
				_ => throw new ArgumentOutOfRangeException()
			};

			UpdateAndDisplayDestPath();
			UpdateStats();
			SameFilenameExists();
		});
    }

	///Generate the destination path for the current item and display it in the GUI.
	private void UpdateAndDisplayDestPath()
	{
		Dispatcher.Invoke(() =>
		{
			if (newDateTaken == null || timePicker.Value == null)
			{
				nextItemBtn.IsEnabled = false;
				destPathText.Text = destFilePath = Path.Combine(unknownDTFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
			}
			else if (newDateTaken != null && timePicker.Value != null)
			{
				nextItemBtn.IsEnabled = true;
				destFolderPath = Path.Combine(destDirRootPath, newDateTaken?.ToString("yyyy/M MMMM/d")!).Replace('\\', '/');
				destPathText.Text = destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
			}
		});
	}

    ///Leaves the current item where it is and loads the next item.
    private void SkipBtn_Click(object sender, RoutedEventArgs e)
    {
	    amountSkipped++;
	    ResumeAutoSort();
    }
    
    ///Undoes any modifications made to the file by the user. The ogFilename and Date Taken are set to what they were when file first loaded.
    private void resetBtn_Click(object sender, RoutedEventArgs e)
    {
        filenameTextBox.Text = ogFilename;
        datePicker.SelectedDate = newDateTaken = ogDateTaken;
        if (ogDateTaken == null)
        {
            ogDateTakenLabel.Content = "None";
            newDateTakenLabel.Content = "None";
        }
        else if (ogDateTaken != null)
        {
            datePicker.DisplayDate = (DateTime) ogDateTaken;
            ogDateTakenLabel.Content = ogDateTaken?.ToString("M/d/yyyy h:mm:ss tt");
            newDateTakenLabel.Content = $"{newDateTaken?.ToString("M/d/yyyy")} {timePicker.Value?.ToString(" h:mm:ss tt")}";
        }
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
		amountDeleted++;
        ResumeAutoSort();
    }

    ///Moves the current item to its new home and loads the next item.
    private void nextItemBtn_Click(object sender, EventArgs e)
    {
        MoveItem(false);
    }
    
    ///Sends the current item to the Unknown Date Taken folder and loads the next item.
    private void UnknownDateBtn_OnClick(object sender, RoutedEventArgs e)
    {
        MoveItem(true);
    }

    /// <summary>Called when either next button or unknown DT button clicked. Moves the current item to its new location (destFilePath) by spawning a Thread.</summary>
    /// <param name="unknownDT">Pass in 'true' if this item will be sent to the Unknown Date Taken folder. False if it will be sent to a sorted folder (determined in UpdateDestPath()).</param>
    private void MoveItem(bool unknownDT)
    {
	    if (unknownDT) destFilePath = Path.Combine(unknownDTFolderPath, Path.GetFileName(currItemFullPath));

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

            //Only needed here because if an item with the same exact path already existed, no need to re-add.
            Dispatcher.Invoke(() => destDirContents.TryAdd(filenameTextBox.Text, destFilePath.Replace(unknownDT ? unknownDTFolderPath : destFolderPath, null)));
        }
        amountSorted++;
        
        //Stupid but fixes file in use error
        GC.Collect();
        GC.WaitForPendingFinalizers();

        //Creating these string variables prevents file in use errors (I think).
        string movePath = currItemFullPath;
        string destPath = destFilePath;
        
        //Yes this part is stupid, but I don't care.
        DateTime? combined = null; //Combination of the date picker AND the time picker.
		
	    Dispatcher.Invoke(() =>
	    {
		    if (newDateTaken != null && timePicker.Value != null)
			    combined = new DateTime(newDateTaken.Value.Year, newDateTaken.Value.Month, newDateTaken.Value.Day, timePicker.Value.Value.Hour, timePicker.Value.Value.Minute, timePicker.Value.Value.Second);
	    });

        Task.Run(() => MoveAndUpdate(ogDateTaken, combined ?? newDateTaken));
        ResumeAutoSort();

        void MoveAndUpdate(DateTime? ogDT, DateTime? newDT)
        {
	        Dispatcher.Invoke(() =>
	        {
		        if (settings.updateDTOnSort.IsChecked == true)
		        {
			        //Only clear DT metadata if user chooses to do so when AS paused (or not running) and they hit the Unknown DT button.
			        //If AutoSort didn't find any DT metadata, then it doesn't make sense to run the command.
			        if (unknownDT && (autoSortSuspended || settings.autoSortCheckBox.IsChecked == false))
				        D.UpdateDateTaken(movePath, null);
			        else if (ogDT != newDT || dateTakenSrc == D.DateTakenSrc.Filename && settings.updateMetadataWithFilenameDT.IsChecked == true)
				        D.UpdateDateTaken(movePath, newDT);
		        }
		    });

	        File.Move(movePath, destPath);
        }
    }
    
    ///Called in places where AutoSort can be suspended to perform necessary cleanup before resuming AutoSort.
    private void ResumeAutoSort()
    {
	    Dispatcher.Invoke(() =>
	    {
		    progressBar.Value++;

		    if (settings.autoSortCheckBox.IsChecked == true)
		    {
			    autoSortSuspended = false;
			    currentItemGroupBox.IsEnabled = false;
			    itemPreview.Source = null;
			    originalPathText.Text = destPathText.Text = filenameTextBox.Text = null;
			    ogDateTakenLabel.Content = dateTakenSrcLabel.Content = newDateTakenLabel.Content = null;
			    datePicker.SelectedDate = timePicker.Value = null;
			    UpdateStats();
		    }
		    else
		    {
			    if (unsortedFiles.Count > 0) LoadAndDisplayNextItem();
			    else if (unsortedFiles.Count == 0) Cleanup();
		    }
	    });
    }

	///<summary>Runs garbage collection and recycles the file specified</summary>
    private static void RecycleFile(string path)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        Task.Run(() => FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin)); //https://stackoverflow.com/a/3282456
    }

    private void OriginalPathText_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //https://stackoverflow.com/a/1132559
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = Path.GetDirectoryName(currItemFullPath),
            UseShellExecute = true,
            Verb = "open"
        });
    }
    
    ///Performs cleanup tasks upon sorting completion, mostly for preparing for another sort, if necessary.
    private void Cleanup()
    {
        Dispatcher.Invoke(() =>
        {
	        unsortedFiles.Clear();
	        destDirContents.Clear();
	        itemPreview.LoadedBehavior = MediaState.Manual;
            itemPreview.Source = null;
            itemPreview.Stop();
            warningText.Text = null;
            datePicker.SelectedDate = timePicker.Value = null;
            muteUnmuteBtn.IsEnabled = false;
            currentItemGroupBox.IsEnabled = false;
            setupGroupBox.IsEnabled = true;
            srcDirRootPath = destDirRootPath = ext = "";
            ogFilename = destFolderPath = destFilePath = "";
            filenameTextBox.Text = null;
            originalPathText.Text = destPathText.Text = null;
            ogDateTakenLabel.Content = newDateTakenLabel.Content = dateTakenSrcLabel.Content = null;
            srcDirLabel.Content = destDirLabel.Content = "";
            srcDirRootPath = destDirRootPath = unknownDTFolderPath = "";
        
			statsLabel.Content = $"{amountSorted} Sorted    {amountSkipped} Skipped    {amountDeleted} Deleted    0 Left    {amountSorted + amountSkipped + amountDeleted} Total";
			amountSorted = amountSkipped = amountDeleted = 0;
        });

        GC.Collect();
		GC.WaitForPendingFinalizers();
	}

	private void UpdateStats() => Dispatcher.Invoke(() => statsLabel.Content = $"{amountSorted} Sorted    {amountSkipped} Skipped    {amountDeleted} Deleted    {unsortedFiles.Count + 1} Left    {amountSorted + amountSkipped + amountDeleted + unsortedFiles.Count + 1} Total"); //The + 1 is necessary to include the current item user is looking at towards how many are left to sort.

	private void MuteUnmuteBtn_Click(object sender, RoutedEventArgs e)
	{
        itemPreview.IsMuted = !itemPreview.IsMuted;
        muteUnmuteBtn.Content = itemPreview.IsMuted ? "Un_mute" : "_Mute";
    }

	///Perform validation of filename user entered.
	private void filenameTextBox_TextChanged(object sender, EventArgs e)
	{
		if (destDirContents.ContainsKey(filenameTextBox.Text + ext))
		{
			warningText.Text = $"A file with the same name already exists at {destDirContents[filenameTextBox.Text + ext]}";
		}
		else if (filenameTextBox.Text.Any(c => invalidChars.Contains(c)))
		{
			nextItemBtn.IsEnabled = unknownDateBtn.IsEnabled = false;
			destPathText.Foreground = Brushes.Red;
			destPathText.Text = "Filename cannot contain \\ / : * ? \" < > |";
		}
		else if (String.IsNullOrEmpty(filenameTextBox.Text))
		{
			nextItemBtn.IsEnabled = unknownDateBtn.IsEnabled = false;
			destPathText.Foreground = Brushes.Red;
			destPathText.Text = "Filename cannot be blank";
		}
		else
		{
			warningText.Text = null;
			nextItemBtn.IsEnabled = unknownDateBtn.IsEnabled = true;
			destPathText.Foreground = Brushes.Black;
			UpdateAndDisplayDestPath();
		}
	}

	private void ValidateNewDateTaken()
    {
	    if (datePicker.SelectedDate == null)
	    {
		    ogDateTakenLabel.Content = "None";
		    newDateTaken = null;
	    }
	    else if (datePicker.SelectedDate != null && timePicker.Value != null)
	    {
		    newDateTaken = new DateTime(datePicker.SelectedDate.Value.Year, datePicker.SelectedDate.Value.Month, datePicker.SelectedDate.Value.Day, timePicker.Value!.Value.Hour, timePicker.Value!.Value.Minute, timePicker.Value!.Value.Second);
		    newDateTakenLabel.Content = $"{newDateTaken?.ToString("M/d/yyyy")} {timePicker.Value?.ToString(" h:mm:ss tt")}";
		    datePicker.DisplayDate = (DateTime) datePicker.SelectedDate;
	    }
        
	    UpdateAndDisplayDestPath();
    }

    private void DatePicker_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e) => ValidateNewDateTaken();

    private void TimePicker_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) => ValidateNewDateTaken();
    
    ///Checks the filename to see if a file with the same name exists in the destination folder. Returns true if so and also sets warning text.
    private bool SameFilenameExists()
    {
	    bool returnVal = false;
	    string filename = Path.GetFileName(currItemFullPath);
	    Dispatcher.Invoke(() =>
	    {
		    if (destDirContents.ContainsKey(filename))
		    {
			    warningText.Text = $"A file with the same name already exists at {destDirContents[filename]}";
			    returnVal = true;
		    }
		    else
		    {
			    warningText.Text = null;
			    returnVal = false;
		    }
	    });
	    return returnVal;
    }
}