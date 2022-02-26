using System.Windows;
using WinForms = System.Windows.Forms;

namespace Graphical_Photo_Organizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GPO : Window
    {
        public GPO()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = dialog.ShowDialog();
        }
    }
}
