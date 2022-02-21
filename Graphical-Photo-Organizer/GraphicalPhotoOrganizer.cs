namespace Graphical_Photo_Organizer;

public partial class GraphicalPhotoOrganizer : Form
{
    private List<string> unsortedFiles = new();
    private string unsortedDirRootPath = "", sortedDirRootPath = "";
    private DateTime dateTaken;
    
    public GraphicalPhotoOrganizer()
    {
        InitializeComponent();
    }

    private void GraphicalPhotoOrganizer_Shown(object sender, EventArgs e)
    {
        srcDirLabel.Text = "";
        destDirLabel.Text = "";
        destPathLabel.Text = "";
        //currentPhotoGroupBox.Enabled = true;
    }

    private void chooseSrcDirBtn_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog(); //https://stackoverflow.com/a/11624322
        fbd.UseDescriptionForTitle = true;
        fbd.Description = "Select Folder of Images to Sort";
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(fbd.SelectedPath) && Directory.Exists(fbd.SelectedPath))
        {
            unsortedFiles = Directory.GetFiles(fbd.SelectedPath, "*.jp*g", SearchOption.AllDirectories).ToList();
            unsortedFiles = unsortedFiles.Concat(Directory.GetFiles(fbd.SelectedPath, "*.png", SearchOption.AllDirectories)).ToList();

            //TODO gui logic here like disabling these controls and opening the rest.
        }
        else if (result != DialogResult.Cancel)
            MessageBox.Show("Unknown error when choosing folder. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void chooseDestDirBtn_Click(object sender, EventArgs e)
    {
        using var fbd = new FolderBrowserDialog();
        fbd.UseDescriptionForTitle = true;
        fbd.Description = "Select Root Folder Where Sorted Items Will Go";
        DialogResult result = fbd.ShowDialog();

        if (result == DialogResult.OK && !String.IsNullOrWhiteSpace(fbd.SelectedPath) && Directory.Exists(fbd.SelectedPath))
        {
            sortedDirRootPath = fbd.SelectedPath;
        }
        else if (result != DialogResult.Cancel)
        {
            MessageBox.Show("Unknown error when choosing folder. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void applyBtn_Click(object sender, EventArgs e)
    {

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

    private void resetBtn_Click(object sender, EventArgs e)
    {

    }
}
