import { IpcMainInvokeEvent, dialog, ipcMain } from "electron";
import { promises as fs } from "fs";
import * as path from "path";
import { UnsortedFile, createUnsortedFile } from "../types/UnsortedFile";
import { mainWindow } from "./main";

export const supportedImageExtensions = new Set([".jpg", ".jpeg", ".png", ".gif"]);
export const supportedVideoExtensions = new Set([".mp4", ".mov", ".mkv"]);
export const supportedExtensions = new Set([...supportedImageExtensions, ...supportedVideoExtensions]);

export async function getFiles(dirPath: string): Promise<string[]> {
    const entries = await fs.readdir(dirPath, { withFileTypes: true });
    const files = await Promise.all(
        entries.map(async (entry) => {
            const fullPath = path.join(dirPath, entry.name);
            return entry.isDirectory() ? await getFiles(fullPath) : fullPath;
        })
    );
    return files.flat();
}

console.log("Begin setting up API");

ipcMain.handle("files:getFiles", async (event: IpcMainInvokeEvent, dir: string): Promise<string[]> => {
    if (!dir) return [];
    return (await getFiles(dir)).map((file) => file.replaceAll("\\", "/"));
});

ipcMain.handle("files:getUnsortedFiles", async (event: IpcMainInvokeEvent, dir: string): Promise<UnsortedFile[]> => {
    const files = await getFiles(dir);
    const supportedFiles = files.filter((filePath) => supportedExtensions.has(path.extname(filePath).toLowerCase()));
    return await Promise.all(supportedFiles.map((filePath) => createUnsortedFile(filePath)));
});

ipcMain.handle("dialog:openFolder", async (): Promise<string | null> => {
    const result = await dialog.showOpenDialog(mainWindow!, {
        properties: ["openDirectory"],
    });

    if (!result.canceled && result.filePaths.length > 0) {
        return result.filePaths[0];
    }

    return null;
});

ipcMain.handle("files:moveFile", async (event: IpcMainInvokeEvent, oldPath: string, newPath: string): Promise<void> => {
    await fs.rename(oldPath, newPath);
});

console.log("Finish setting up API");
