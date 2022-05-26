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
		S.allowedExts.Clear();

		foreach (FrameworkElement checkbox in extensionsStackPanel.Children)
		{
			CheckBox? currentCheckBox = (CheckBox) checkbox;
			if (currentCheckBox.IsChecked == true)
				S.allowedExts.Add(currentCheckBox.Content.ToString()!);
		}
	}

	private void WhatAreTheseForBtn_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Some files have been encountered (usually memes) that have absurd Date Taken metadata, with years like pre-2000s, 1970, or with years even earlier than that. This can help you catch these during an AutoSort session so they don't get sent to folders where you wouldn't want them.", "What Are These for?", MessageBoxButton.OK, MessageBoxImage.Information);

	private void AutoSortCheckBox_Click(object sender, RoutedEventArgs e)
	{
		if (autoSortCheckBox.IsChecked == null) return;
		autoSortSP.IsEnabled = (bool) autoSortCheckBox.IsChecked;
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
}