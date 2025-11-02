import { app } from "electron";
import { ExifTool } from "exiftool-vendored";
import fs from "fs";
import { fileURLToPath } from "node:url";
import path from "path";

function getExifToolPath() {
    if (app.isPackaged) {
        return path.join(process.resourcesPath, "ExifTool", "exiftool.exe");
    } else {
        return path.join(path.dirname(fileURLToPath(import.meta.url)), "../bin/exiftool/exiftool.exe");
    }
}

const exePath = getExifToolPath();

if (!fs.existsSync(exePath)) {
    console.error("ExifTool executable not found at:", exePath);
}

export const exifTool = new ExifTool({
    exiftoolPath: exePath,
});
