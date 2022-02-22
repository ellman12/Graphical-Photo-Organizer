using M = Graphical_Photo_Organizer.Metadata;

namespace Graphical_Photo_Organizer;

public partial class GraphicalPhotoOrganizer : Form
{
    //Set during setup
    private List<string> unsortedFiles = new();
    private string unsortedDirRootPath = "", sortedDirRootPath = "";

    //Set when new file selected.
    private string filename = "", destPath = "";
    private DateTime dateTaken;
    private M.DateTakenSrc dateTakenSrc;

    public GraphicalPhotoOrganizer()
    {
        InitializeComponent();
    }

    private void GraphicalPhotoOrganizer_Shown(object sender, EventArgs e)
    {
        srcDirLabel.Text = "";
        destDirLabel.Text = "";
        destPathLabel.Text = "";

        //TODO: temporary stuff to make development easier
        srcDirLabel.Text = unsortedDirRootPath = "C:/Users/Elliott/Pictures/unsorted pics";
        destDirLabel.Text = sortedDirRootPath = "C:/Users/Elliott/Pictures/sorted pics";

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
    }

    private void LoadImage(string path)
    {
        filename = Path.GetFileName(path);
        (bool hasData, dateTakenSrc) = M.GetDateTaken(path, out dateTaken);

        if (!hasData)
        {
            dateTaken = DateTime.Now;
            dateTakenSrc = M.DateTakenSrc.Now;
        }

        filenameTextBox.Text = filename;
        dateTakenLabel.Text = dateTaken.ToString("M/d/yyyy");
        dateTakenSrcLabel.Text = "Source: " + dateTakenSrc;
        datePicker.SelectionStart = DateTime.Today;
        photoPreview.ImageLocation = path;
    }

    private void nextPhotoBtn_Click(object sender, EventArgs e)
    {
    }

    private void deletePhotoBtn_Click(object sender, EventArgs e)
    {
    }

    private void skipPhotoBtn_Click(object sender, EventArgs e)
    {
    }

    ///<summary>Reset changes made to this photo. Resets values to what they first were.</summary>
    private void resetBtn_Click(object sender, EventArgs e)
    {
        filenameTextBox.Text = filename;
        dateTakenLabel.Text = dateTaken.ToString("M/d/yyyy");
        datePicker.SelectionStart = DateTime.Today;
    }
}