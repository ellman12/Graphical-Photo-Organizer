# Graphical-Photo-Organizer
A convenient C# WPF utility for organizing photos and videos based on when they were taken.

Problem: Your folder of pictures and videos looks like this:
![image](https://user-images.githubusercontent.com/56001219/156902615-aad8f019-0719-4e37-ac2a-bd57f4133d89.png)

But what you really want is to have it look like this:
![image](https://user-images.githubusercontent.com/56001219/156902653-2a078b6f-750b-44d3-9f57-0ef58215458a.png)

And inside each year folder something like this:
![image](https://user-images.githubusercontent.com/56001219/156902676-0b3c4f65-1c29-48d7-821f-9f93124d69a1.png)

With Graphical Photo Organizer (GPO), organizing messy folders of pictures and videos like this is a piece of cake! 🍰

## Installation
Check the [Releases](https://github.com/ellman12/Graphical-Photo-Organizer/releases) and download the latest version. **GPO requires .NET 6**.<br>
Unzip the file and find the .exe. From there it should be able to run wherever you downloaded it to.

## Using Graphical Photo Organizer
GPO was designed to be fast, simple, and easy to use. What follows is a guide for your first time use.<br>
1. In the first GroupBox, choose the folder you want to sort.
2. Choose where sorted items will end up.
3. Once you've done that, hit the begin button to start sorting. One important thing to note is you can quit sorting whenever you want; GPO moves each file individually as you sort so there's no need to "save" or "apply" or anything.

![image](https://user-images.githubusercontent.com/56001219/156902959-84fe5dcf-a109-40e5-9860-68584933bb37.png)
When you're viewing a single item, you can change its filename, and also when it was taken (thus affecting where it will end up).
Also when viewing an item, you can
* Skip it and not move it
* Delete the file
* Reset any changes made
* Move the file to its new location and advanced to the next file

The text next to the OG (original) date taken is where it found the date taken data, which can either be Metadata, the Filename, or if neither of those, the date right now.

And I think that's it. GPO is designed to be as simple as possible, while still being fast and efficient.
Feel free to open a PR if you spot a bug 🐛 or have a feature idea 💡.

## Contributing to Graphical Photo Organizer
To contribute to Graphical Photo Organizer, follow these steps:

1. Fork this repository.
2. Create a branch: `git checkout -b <branch_name>`.
3. Make your changes and commit them: `git commit -m '<commit_message>'`
4. Push to the original branch: `git push origin Graphical-Photo-Organizer/<location>`
5. Create the pull request.

Alternatively see the GitHub documentation on [creating a pull request](https://help.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request).

## Contact
If you want to contact me you can reach me at either ellduc4@gmail.com or elliott.ducharme@trojans.dsu.edu
