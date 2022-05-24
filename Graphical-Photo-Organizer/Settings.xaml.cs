using System.Windows;
using System.Windows.Controls;
using S = Graphical_Photo_Organizer.Shared;

namespace Graphical_Photo_Organizer
{
	///Interaction logic for Settings.xaml
	public partial class Settings
	{
		public Settings() => InitializeComponent();

		///Clears and then populates the HashSet&lt;string&gt; of allowed extensions (GPO.allowedExts) based on which CheckBoxes are checked in Settings.
		private void ExtCheckbox_Click(object sender, RoutedEventArgs e)
		{
			S.allowedExts.Clear();

			foreach (FrameworkElement checkbox in extensionsStackPanel.Children)
			{
				CheckBox? currentCheckBox = (CheckBox) checkbox;
				if (currentCheckBox.IsChecked == true)
					S.allowedExts.Add(currentCheckBox.Content.ToString()!);
			}
		}
	}
}