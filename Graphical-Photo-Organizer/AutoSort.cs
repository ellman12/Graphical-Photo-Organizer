namespace Graphical_Photo_Organizer;

//The part of GPO just for AutoSort stuff.
public partial class GPO
{
	///<summary>Starts AutoSort and determines which method to use based on user's choices in Settings GUI.</summary>
	///<remarks>How to update controls on another thread: https://stackoverflow.com/a/9732853</remarks>
	private void AutoSort()
	{
		if (settings.sendToUnknownDTBtn.IsChecked == true) AutoSortSendToUnknownFolder();
		else if (settings.promptBtn.IsChecked == true) AutoSortPromptNullDT();
		else if (settings.skipItemBtn.IsChecked == true) AutoSortUnknownDTSkip();
	}
	
	///Run AutoSort and if encounter an item with null/unknown DT, automatically send it to Unknown DT folder without telling user.
	private void AutoSortSendToUnknownFolder()
	{
		
	}
	
	///Run AutoSort and if encounters an item with null/unknown DT, pause AutoSort and ask user what to do with the item.
	private void AutoSortPromptNullDT()
	{
		
	}
	
	
	///Run AutoSort and if encounter an item with null/unknown DT, skip the item automatically.
	private void AutoSortUnknownDTSkip()
	{
	}

	///Generate the destination path for the current item without displaying it in the GUI.
	private void UpdateDestPath()
	{
		if (newDateTaken == null)
			destFilePath = Path.Combine(unknownDTFolderPath, filenameTextBox.Text + ext);
		else if (newDateTaken != null)
			destFilePath = Path.Combine(destDirRootPath, newDateTaken?.ToString("yyyy/M MMMM/d")!, filenameTextBox.Text + ext);
	}
}