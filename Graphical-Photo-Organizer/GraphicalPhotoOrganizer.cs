using M = Graphical_Photo_Organizer.Metadata;

namespace Graphical_Photo_Organizer;

public partial class GraphicalPhotoOrganizer : Form
{
    //Set during setup
    private List<string> unsortedFiles = new();
    private string unsortedDirRootPath = "", sortedDirRootPath = "";

    //Set when new file selected or when user updates data.
    private string filename = "", ext = "";
    private string destFolderPath = ""; //Folder where the current photo will end
    private string destFilePath = ""; //The full final path to it.
    private DateTime dateTaken;
    private M.DateTakenSrc dateTakenSrc;

    //Stats
    private int amountSorted = 0;
    private int amountSkipped = 0;
    private int amountDeleted = 0;

    public GraphicalPhotoOrganizer()
    {
        InitializeComponent();
    }

    private void GraphicalPhotoOrganizer_Shown(object sender, EventArgs e)
    {
        srcDirLabel.Text = "";
        destDirLabel.Text = "";
        originalPathLabel.Text = "";
        destPathLabel.Text = "";
        statsLabel.Text = "";

        //TODO: temporary stuff to make development easier
        srcDirLabel.Text = unsortedDirRootPath = "C:/Users/Elliott/Pictures/tmp/unsorted pics";
        destDirLabel.Text = sortedDirRootPath = "C:/Users/Elliott/Pictures/tmp/sorted pics";

        beginBtn.Enabled = true;
        currentPhotoGroupBox.Enabled = true;

        unsortedFiles = Directory.GetFiles(unsortedDirRootPath, "*.jp*g", SearchOption.AllDirectories).ToList();
        unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(unsortedDirRootPath, "*.png", SearchOption.AllDirectories)).ToList();
    }

    private void chooseSrcDirBtn_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog(); //https://stackoverflow.com/a/11624322
        fbd.UseDescriptionForTitle = true;
        fbd.Description = "Select Folder of Images to Sort";
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
            unsortedFiles = Directory.GetFiles(fbd.SelectedPath, "*.jp*g", SearchOption.AllDirectories).ToList();
            unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(fbd.SelectedPath, "*.png", SearchOption.AllDirectories)).ToList();

            unsortedDirRootPath = fbd.SelectedPath;
            srcDirLabel.Text = unsortedDirRootPath;
        }
        else if (result != DialogResult.Cancel)
            MessageBox.Show("Unknown error when choosing folder. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        ValidateFolderDirs();
    }

    private void chooseDestDirBtn_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        fbd.UseDescriptionForTitle = true;
        fbd.Description = "Select Root Folder Where Sorted Items Will Go";
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
            sortedDirRootPath = fbd.SelectedPath;
            destDirLabel.Text = sortedDirRootPath;
        }
        else if (result != DialogResult.Cancel)
            MessageBox.Show("Unknown error when choosing folder. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        ValidateFolderDirs();
    }

    ///<summary>Checks the folder paths the user chose to make sure they are valid and they can proceed to the sorting.</summary>
    private void ValidateFolderDirs()
    {
        if (unsortedFiles.Count == 0)
        {
            MessageBox.Show("There were no pictures found in the source folder provided.", "No pictures found in folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
            beginBtn.Enabled = false;
        }
        else if (unsortedDirRootPath == sortedDirRootPath)
        {
            MessageBox.Show("You cannot have the same source and destination folder path!", "Same folder path specified", MessageBoxButtons.OK, MessageBoxIcon.Error);
            beginBtn.Enabled = false;
        }
        else if (!String.IsNullOrWhiteSpace(unsortedDirRootPath) && !String.IsNullOrWhiteSpace(sortedDirRootPath))
        {
            beginBtn.Enabled = true;
        }
        else
        {
            currentPhotoGroupBox.Enabled = false;
            beginBtn.Enabled = false;
        }
    }

    //https://stackoverflow.com/a/1132559
    private void srcDirLabel_Click(object sender, EventArgs e) =>
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = unsortedDirRootPath,
            UseShellExecute = true,
            Verb = "open"
        });

    private void destDirLabel_Click(object sender, EventArgs e) =>
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = sortedDirRootPath,
            UseShellExecute = true,
            Verb = "open"
        });

    private void beginBtn_Click(object sender, EventArgs e)
    {
        currentPhotoGroupBox.Enabled = true;
        LoadImage(unsortedFiles[0]);
        setupGroupBox.Enabled = false;
        UpdateStats();
    }

    private void LoadImage(string path)
    {
        originalPathLabel.Text = unsortedFiles[0];
        filename = Path.GetFileNameWithoutExtension(path);
        ext = Path.GetExtension(path);

        (bool hasData, dateTakenSrc) = M.GetDateTaken(path, out dateTaken);

        if (!hasData)
        {
            dateTaken = DateTime.Now;
            dateTakenSrc = M.DateTakenSrc.Now;
        }

        filenameTextBox.Text = filename;
        dateTakenLabel.Text = dateTaken.ToString("M/d/yyyy");
        dateTakenSrcLabel.Text = "Source: " + dateTakenSrc;
        datePicker.SelectionStart = dateTaken;
        photoPreview.ImageLocation = path;

        UpdateDestPath();
        CheckForDuplicates(path);
    }

    private void CheckForDuplicates(string path)
    {
        string[] sortedFiles = Directory.GetFiles(sortedDirRootPath, "*.jp*g", SearchOption.AllDirectories);
        sortedFiles = sortedFiles.Concat(Directory.GetFiles(sortedDirRootPath, "*.png", SearchOption.AllDirectories)).ToArray();

        foreach (string file in sortedFiles)
        {
            if (file.EndsWith(Path.GetFileName(path)))
                MessageBox.Show("The current file might already be in the sorted folder at the path\n " + file.Replace('\\', '/') + "\nA file with the same name already is in the sorted folder.", "Possible Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void datePicker_DateChanged(object sender, DateRangeEventArgs e)
    {
        dateTakenLabel.Text = datePicker.SelectionStart.ToString("M/d/yyyy");
        UpdateDestPath();
    }

    /// <summary>Moves the current item to its new home and loads the next image.</summary>
    private void nextPhotoBtn_Click(object sender, EventArgs e)
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
            DialogResult result = MessageBox.Show("A file with the same name already exists at that location. Overwrite it with this file?\nYes will overwrite it with this file, No will keep the original file and move on to the next file to sort, Cancel will cancel this.", "File already exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);

            if (result == DialogResult.Yes)
            {
                File.Delete(destFilePath);
                File.Move(unsortedFiles[0], destFilePath);
            }
            else if (result == DialogResult.No)
            {
                LoadNextImage();
                return;
            }
            else if (result == DialogResult.Cancel)
                return;
        }
        
        LoadNextImage();
    }

    ///<summary>Removes the current image from the List and loads the next one.</summary>
    private void LoadNextImage()
    {
        unsortedFiles.RemoveAt(0);
        amountSorted++;
        UpdateStats();
        
        if (unsortedFiles.Count > 0)
            LoadImage(unsortedFiles[0]);
    }

    ///<summary>Updates the folder where the current photo will be sent and also its final path and the label that displays the full path.</summary>
    private void UpdateDestPath()
    {
        destFolderPath = Path.Combine(sortedDirRootPath, datePicker.SelectionStart.ToString("yyyy/M MMMM/d")).Replace('\\', '/');
        destFilePath = Path.Combine(destFolderPath, filenameTextBox.Text + ext).Replace('\\', '/');
        destPathLabel.Text = destFilePath;
    }

    private void filenameTextBox_TextChanged(object sender, EventArgs e) => UpdateDestPath();

    private void deletePhotoBtn_Click(object sender, EventArgs e)
    {
        DialogResult result = MessageBox.Show("Are you sure you want to delete this photo?", "Delete this photo?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
        {
            File.Delete(unsortedFiles[0]);
            unsortedFiles.RemoveAt(0);
            LoadImage(unsortedFiles[0]);
            amountDeleted++;
            UpdateStats();
        }
    }

    private void skipPhotoBtn_Click(object sender, EventArgs e)
    {
        unsortedFiles.RemoveAt(0);
        LoadImage(unsortedFiles[0]);
        amountSkipped++;
        UpdateStats();
    }

    private void photoPreview_Click(object sender, EventArgs e)
    {
        Image image = photoPreview.Image;
        
        //https://stackoverflow.com/a/19448432
        //https://docs.microsoft.com/en-us/dotnet/api/system.drawing.image.rotateflip?view=dotnet-plat-ext-6.0
        MouseEventArgs me = (MouseEventArgs) e;
        if (me.Button == MouseButtons.Left)
            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        else if (me.Button == MouseButtons.Right)
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            
        photoPreview.Image = image;
    }

    private void UpdateStats() => statsLabel.Text = $"Amount Sorted: {amountSorted}    Amount Skipped: {amountSkipped}    Amount Deleted: {amountDeleted}    Amount Left: {unsortedFiles.Count}";

    ///<summary>Reset changes made to this photo. Resets values to what they first were.</summary>
    private void resetBtn_Click(object sender, EventArgs e)
    {
        filenameTextBox.Text = filename;
        dateTakenLabel.Text = datePicker.SelectionStart.ToString("M/d/yyyy");
        datePicker.SelectionStart = dateTaken;
    }
}