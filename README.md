# Graphical-Photo-Organizer
### **A C# WPF utility for organizing photos and videos based on when they were taken.**

Problem: Your folder of pictures and videos looks like this:<br>
![image](https://user-images.githubusercontent.com/56001219/156902615-aad8f019-0719-4e37-ac2a-bd57f4133d89.png)

But what you really want is to have it sorted by year, month, and day, like this:<br>
![image](https://user-images.githubusercontent.com/56001219/173249817-dd167f7b-56c2-4ece-a26e-058fc0b88c90.png)
![image](https://user-images.githubusercontent.com/56001219/173249826-015c14df-62b9-442c-b9f4-a5935455438f.png)
![image](https://user-images.githubusercontent.com/56001219/173249835-1f3d3151-40c0-4bf0-b64b-d60a62a0dc8d.png)
![image](https://user-images.githubusercontent.com/56001219/173249843-be9d69a5-55fd-4ac6-a4e6-3310069bc411.png)

With Graphical Photo Organizer (GPO), organizing messy folders of pictures and videos is a piece of cake! 🍰

## Installation
1. Download the [ExifTool](https://exiftool.org/) Windows Executable. This is required for modifying Date Taken metadata.
	1. Change the name of the .exe from `exiftool(-k).exe` to `exiftool.exe`
	2. Add exiftool.exe to your system's `PATH` or move it to a folder already in the `PATH`, like `C:/Windows`.
2. Check the [Releases](https://github.com/ellman12/Graphical-Photo-Organizer/releases) and in the latest version of GPO, download the `GPO.zip` file.
3. Unzip the file and find `Graphical-Photo-Organizer.exe`. Double click to run it. It should be able to run wherever you put it.

## Using Graphical Photo Organizer
GPO supports several common photo and video file types:
* JPG/JPEG
* PNG
* GIF
* MP4
* MKV
* MOV

GPO was designed to be fast, simple, and easy to use. What follows is a brief overview on how to use it, with much more info and detail in the [Wiki](https://github.com/ellman12/Graphical-Photo-Organizer/wiki).<br>

### Basic Setup for Manual Sorting
The most basic sorting method is by doing it manually, one file at a time. This can take a very long time to do by hand, but if you want full control over where each item will go, what it will be renamed to, what its internal Date Taken metadata will be set to, and more, then manual mode is the way to go.
1. In Setup, choose the folder you want to sort (AKA the source folder).
2. Choose where sorted items will end up (AKA the destination folder).
4. Once you've done that, hit the begin button to start sorting. One important thing to note is you can quit sorting whenever you want; GPO moves each file individually as you sort so there's no need to "save" or "apply" or anything.

![image](https://user-images.githubusercontent.com/56001219/173248843-880b537f-6e99-4e51-bb68-841806262cdb.png)

When you're viewing a single item like this, you can change its filename, and also when it was taken (thus affecting where it will end up).
Also when viewing an item, you can...
* Move the file to its new location and advance to the next file
* Skip it and advance to the next file
* Delete the file and advance to the next file
* Reset any changes made to the current file

GPO also has several keyboard shortcuts. Press Alt + ...
* B to Begin Sorting
* N for Next Item
* M to Mute/unmute Video
* U to move the current item to the Unknown Date Taken folder
* S to Skip the Current Item
* D to Delete the Current Item
* R to Reset Changes to Current Item

The colored text below the OG (original) Date Taken is where GPO found the Date Taken data, which can either be 'Metadata' (green), 'Filename' (yellow), or if no Date Taken data found, 'None' (red).

### Basic Setup for AutoSort
AutoSort is a new feature in V1.2 that can automatically sort a folder, making the process of sorting _way_ faster and easier. It does take a little bit of setup, but once it starts, it can chew through a folder quickly.

The [Wiki](https://github.com/ellman12/Graphical-Photo-Organizer/wiki) goes into a lot more detail about what all the settings are for and when you'd want to use them, but to enable AutoSort, go to the settings window and check the box to turn it on. The default settings should be ideal for almost all use cases. The rest of the AutoSort settings _should_ speak for themselves, but check the Wiki for more info and their use cases.<br>
![image](https://user-images.githubusercontent.com/56001219/173622721-48df6038-4936-4846-8a0e-df2c9e3d245a.png)

## Contributing to Graphical Photo Organizer
To contribute to Graphical Photo Organizer, follow these steps:

1. Fork this repository.
2. Create a branch: `git checkout -b <branch_name>`.
3. Make your changes and commit them: `git commit -m '<commit_message>'`
4. Push to the original branch: `git push origin Graphical-Photo-Organizer/<location>`
5. Create the pull request.

Alternatively see the GitHub documentation on [creating a pull request](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

To compile GPO to a portable .exe using Visual Studio 2022, go under `Build > Publish Selection`, enter these settings in, then click `Publish`:<br>
![image](https://user-images.githubusercontent.com/56001219/173247363-6b6591bb-e0c8-41db-90fc-44d4f1666815.png)

Feel free to open a PR if you spot a bug 🐛 or have a feature idea 💡.

## Contact
If you want to contact me you can reach me at ellduc4@gmail.com
