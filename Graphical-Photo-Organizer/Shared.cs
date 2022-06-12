using System.Collections.Generic;

namespace Graphical_Photo_Organizer;

///Resources shared between the main and Settings windows.
public static class Shared
{
	///Only allow files with these extensions to be sorted. Controlled by user in Settings.
	public static readonly HashSet<string> allowedExts = new(new[] {"jpg", "jpeg", "png", "gif", "mp4", "mov", "mkv"});

	
}