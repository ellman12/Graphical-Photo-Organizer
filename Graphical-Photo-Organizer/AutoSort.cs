using System.IO;
using D = DateTakenExtractor.DateTakenExtractor;

namespace Graphical_Photo_Organizer;

//The part of GPO just for AutoSort stuff.
public partial class GPO
{
	///Run AutoSort and if encounter an item with null/unknown DT, automatically send it to Unknown DT folder without telling user.
	private void AutoSortSendToUnknownFolder()
	{
		while (unsortedFiles.Count > 0)
		{
			currItemFullPath = unsortedFiles.Dequeue();
			newDateTaken = ogDateTaken = D.GetDateTakenAuto(currItemFullPath, out dateTakenSrc);
			UpdateDestPath();
			MoveItem(ogDateTaken == null);
		}
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
			destFilePath = Path.Combine(unknownDTFolderPath, Path.GetFileName(currItemFullPath));
		else if (newDateTaken != null)
		{
			destFolderPath = Path.Combine(destDirRootPath, newDateTaken?.ToString("yyyy/M MMMM/d")!);
			destFilePath = Path.Combine(destFolderPath, Path.GetFileName(currItemFullPath));
		}
	}
}