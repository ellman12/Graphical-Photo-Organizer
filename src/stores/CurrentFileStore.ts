import { format } from "date-fns";
import { create } from "zustand/react";
import { UnsortedFile } from "../types/UnsortedFile";
import { createSelectors } from "./createSelectors";

//Stores the current file being displayed.
interface CurrentFileState {
    currentFile: UnsortedFile | null;
    setCurrentFile: (newFile: UnsortedFile | null) => void;

    newFilename: string;
    setNewFilename: (newName: string) => void;

    newDateTaken: Date | null;
    setNewDateTaken: (newDt: Date | null) => void;

    //Returns a path like rootPath/year/month/day/filename.ext
    getRelativeDestinationPath: (rootPath: string) => string;
}

const useCurrentFileStoreBase = create<CurrentFileState>()((set, get) => ({
    currentFile: null,
    setCurrentFile: (newFile) => set({ currentFile: newFile, newFilename: newFile?.filename ?? "" }),

    newFilename: "",
    setNewFilename: (newName) => set({ newFilename: newName }),

    newDateTaken: null,
    setNewDateTaken: (newDt) => set({ newDateTaken: newDt }),

    getRelativeDestinationPath: (rootPath) => {
        const file = get().currentFile;
        if (!file) return "";

        const dt = get().newDateTaken;
        const filename = get().newFilename;
        const ext = file.extension;

        if (!dt) return `${rootPath}/Unknown/${filename + ext}`;

        return `${rootPath}/${format(dt, "yyyy/MM/dd")}/${filename + ext}`;
    },
}));

export const useCurrentFileStore = createSelectors(useCurrentFileStoreBase);
