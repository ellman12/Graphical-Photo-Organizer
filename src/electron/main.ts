import { BrowserWindow, app } from "electron";
import path from "node:path";
import { fileURLToPath } from "node:url";
import "./api";

const __dirname = path.dirname(fileURLToPath(import.meta.url));

process.env.APP_ROOT = path.join(__dirname, "..");

export const VITE_DEV_SERVER_URL = process.env["VITE_DEV_SERVER_URL"];
export const MAIN_DIST = path.join(process.env.APP_ROOT, "dist-electron");
export const RENDERER_DIST = path.join(process.env.APP_ROOT, "dist");

process.env.VITE_PUBLIC = VITE_DEV_SERVER_URL ? path.join(process.env.APP_ROOT, "public") : RENDERER_DIST;

export let mainWindow: BrowserWindow | null;

function createWindow() {
    mainWindow = new BrowserWindow({
        icon: path.join(process.env.VITE_PUBLIC!, "electron-vite.svg"),
        width: 1280,
        height: 720,
        webPreferences: {
            preload: path.join(__dirname, "preload.mjs"),
            webSecurity: false,
        },
    });
    mainWindow.removeMenu();

    if (VITE_DEV_SERVER_URL) {
        mainWindow.loadURL(VITE_DEV_SERVER_URL);
        mainWindow.webContents.openDevTools();
    } else {
        mainWindow.loadFile(path.join(RENDERER_DIST, "index.html"));
    }
}

app.on("window-all-closed", () => {
    app.quit();
    mainWindow = null;
});

app.on("activate", () => {
    createWindow();
});

app.whenReady().then(createWindow);
