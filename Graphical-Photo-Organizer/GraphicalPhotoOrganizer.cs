namespace Graphical_Photo_Organizer;

public partial class GraphicalPhotoOrganizer : Form
{
    private List<string> unsortedFiles = new();
    
    public GraphicalPhotoOrganizer()
    {
        InitializeComponent();
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
        }
    }
}
