import { contextBridge, ipcRenderer } from "electron";
import { UnsortedFile } from "../types/UnsortedFile";

console.log("Preload loaded");

export const backend = {
    getFiles: async (dir: string): Promise<string[]> => await ipcRenderer.invoke("files:getFiles", dir),
    getUnsortedFiles: async (dir: string): Promise<UnsortedFile[]> => await ipcRenderer.invoke("files:getUnsortedFiles", dir),

    pickFolder: async (): Promise<string | null> => await ipcRenderer.invoke("dialog:openFolder"),
};

contextBridge.exposeInMainWorld("backend", backend);

console.log("backend exposed to main world");
