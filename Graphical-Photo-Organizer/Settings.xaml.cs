using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using S = Graphical_Photo_Organizer.Shared;

namespace Graphical_Photo_Organizer;

///Interaction logic for Settings.xaml
public partial class Settings
{
	public Settings() => InitializeComponent();

	///Clears and then populates the HashSet&lt;string&gt; of allowed extensions (GPO.allowedExts) based on which CheckBoxes are checked in Settings.
	private void ExtCheckbox_Click(object sender, RoutedEventArgs e)
	{
		CheckBox current = (CheckBox) sender;
		S.allowedExts.Clear();

		foreach (FrameworkElement checkbox in extensionsStackPanel.Children)
		{
			CheckBox? currentCheckBox = (CheckBox) checkbox;
			if (currentCheckBox.IsChecked == true)
				S.allowedExts.Add(currentCheckBox.Content.ToString()!);
		}

		if (S.allowedExts.Count == 0)
		{
			MessageBox.Show("At least one extension must be selected to begin sorting.", "Select at least one extension.", MessageBoxButton.OK, MessageBoxImage.Error);
			current.IsChecked = true;
		}
	}
	
	private void UpdateDTOnSort_OnClick(object sender, RoutedEventArgs e) => updateMetadataWithFilenameDT.IsChecked = updateDTOnSort.IsChecked == true;

	private void WhatAreTheseForBtn_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Some files have been encountered (usually memes) that have absurd Date Taken metadata, with years like pre-2000s, 1970, or with years even earlier than that. This can help you catch these during an AutoSort session so they don't get sent to folders where you wouldn't want them.", "What Are These for?", MessageBoxButton.OK, MessageBoxImage.Information);

	private void AutoSortCheckBox_Click(object sender, RoutedEventArgs e)
	{
		if (autoSortCheckBox.IsChecked == null) return;
		autoSortSP.IsEnabled = (bool) autoSortCheckBox.IsChecked;
		
		//Stupid but it works. https://stackoverflow.com/a/13901470
		foreach (Window window in Application.Current.Windows)
			if (window.GetType() == typeof(GPO))
				(window as GPO)!.beginBtn.Content = autoSortCheckBox.IsChecked == true ? "_Begin AutoSort" : "_Begin Manual Sorting";
	}

	private void YearLtCB_OnClick(object sender, RoutedEventArgs e)
	{
		if (yearLtCB.IsChecked == null) return;
		yearLtTB.IsEnabled = (bool) yearLtCB.IsChecked;
	}

	private void YearGtCB_OnClick(object sender, RoutedEventArgs e)
	{
		if (yearGtCB.IsChecked == null) return;
		yearGtTB.IsEnabled = (bool) yearGtCB.IsChecked;
	}

	//Prevents the red X corner button from actually closing the window. Just hides the window.
	protected override void OnClosing(CancelEventArgs e) //https://balajiramesh.wordpress.com/2008/07/24/hide-a-window-instead-of-closing-it-in-wpf/
	{
		e.Cancel = true;
		Visibility = Visibility.Collapsed;
	}
}