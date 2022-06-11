using System;
using System.IO;
using System.Threading;
using D = DateTakenExtractor.DateTakenExtractor;

namespace Graphical_Photo_Organizer;

//The part of GPO just for AutoSort stuff.
public partial class GPO
{
	///Used for the 'Prompt if DT null' AutoSort mode so the AutoSort Thread waits for the user to decide what to do with the null DT item.
	private bool autoSortSuspended;
	
	///Run AutoSort and if encounter an item with null/unknown DT, automatically send it to Unknown DT folder without telling user.
	private void AutoSortSendToUnknownFolder()
	{
		while (unsortedFiles.Count > 0)
		{
			if (autoSortSuspended)
			{
				Thread.Sleep(500);
				continue;
			}
			
			currItemFullPath = unsortedFiles.Dequeue();
			newDateTaken = ogDateTaken = D.GetDateTakenAuto(currItemFullPath, out _);

			bool sameFilename = SameFilenameExists();
			bool dtYearInvalid = DateTakenYearInvalid();
			if (sameFilename || dtYearInvalid)
			{
				Dispatcher.Invoke(() => autoSortSuspended = currentItemGroupBox.IsEnabled = true);
				LoadAndDisplayItem(currItemFullPath);
				continue;
			}
			
			UpdateDestPath();
			MoveItem(ogDateTaken == null);
		}
	}

	///Run AutoSort and if encounters an item with null/unknown DT, pause AutoSort and ask user what to do with the item.
	private void AutoSortPromptNullDT()
	{
		while (unsortedFiles.Count > 0)
		{
			if (autoSortSuspended)
			{
				Thread.Sleep(500);
				continue;
			}
			
			currItemFullPath = unsortedFiles.Dequeue();
			newDateTaken = ogDateTaken = D.GetDateTakenAuto(currItemFullPath, out _);

			bool sameFilename = SameFilenameExists();
			bool dtYearInvalid = DateTakenYearInvalid(); 
			if (sameFilename || dtYearInvalid)
			{
				Dispatcher.Invoke(() => autoSortSuspended = currentItemGroupBox.IsEnabled = true);
				LoadAndDisplayItem(currItemFullPath);
				continue;
			}
			
			if (ogDateTaken == null)
			{
				Dispatcher.Invoke(() => currentItemGroupBox.IsEnabled = true);
				LoadAndDisplayItem(currItemFullPath);
				autoSortSuspended = true;
			}
			else if (ogDateTaken != null)
			{
				UpdateDestPath();
				MoveItem(false);
			}
		}	
	}

	///Run AutoSort and if encounter an item with null/unknown DT, skip the item.
	private void AutoSortUnknownDTSkip()
	{
		while (unsortedFiles.Count > 0)
		{
			if (autoSortSuspended)
			{
				Thread.Sleep(500);
				continue;
			}
			
			currItemFullPath = unsortedFiles.Dequeue();
			newDateTaken = ogDateTaken = D.GetDateTakenAuto(currItemFullPath, out _);

			bool sameFilename = SameFilenameExists();
			bool dtYearInvalid = DateTakenYearInvalid(); 
			if (sameFilename || dtYearInvalid)
			{
				Dispatcher.Invoke(() => autoSortSuspended = currentItemGroupBox.IsEnabled = true);
				LoadAndDisplayItem(currItemFullPath);
				continue;
			}

			if (ogDateTaken == null)
			{
				amountSkipped++;
			}
			else if (ogDateTaken != null)
			{
				UpdateDestPath();
				MoveItem(false);
			}
		}
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

	///<summary>If the setting is enabled, checks the Date Taken year value of the current item and pauses AutoSort so user can decide what to do with the item.</summary>
	///<returns>True if Date Taken year is invalid, false if valid.</returns>
	private bool DateTakenYearInvalid()
	{
		if (ogDateTaken == null) return false;
		bool returnVal = false;
		
		Dispatcher.Invoke(() =>
		{
			if (settings.yearLtCB.IsChecked == true && Int32.TryParse(settings.yearLtTB.Text, out int validYearValue) && ogDateTaken?.Year < validYearValue)
			{
				statusTextBlock.Text = $"This item's DT year of {ogDateTaken?.Year} is less than value in Settings. Choose what to do with it.";
				returnVal = true;
			}
			else if (settings.yearGtCB.IsChecked == true && Int32.TryParse(settings.yearGtTB.Text, out validYearValue) && ogDateTaken?.Year > validYearValue)
			{
				statusTextBlock.Text = $"This item's DT year of {ogDateTaken?.Year} is greater than value in Settings. Choose what to do with it.";
				returnVal = true;
			}
			else
			{
				statusTextBlock.Text = null;
				returnVal = false;
			}
		});
		return returnVal;
	}
}