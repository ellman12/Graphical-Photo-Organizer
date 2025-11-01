import { create } from "zustand/react";
import { UnsortedFile } from "../types/UnsortedFile";
import { createSelectors } from "./createSelectors";

//Stores data used for the current sorting operation.
interface SortingStore {
    sourceDir: string;
    setSourceDir: (sourceDir: string) => void;

    destinationDir: string;
    setDestinationDir: (destinationDir: string) => void;

    unsortedFiles: UnsortedFile[];
    setUnsortedFiles: (files: UnsortedFile[]) => void;
}

const useSortingStoreBase = create<SortingStore>()((set) => ({
    sourceDir: "",
    setSourceDir: (sourceDir) => set({ sourceDir: sourceDir.replaceAll("\\", "/") }),
    destinationDir: "",
    setDestinationDir: (destinationDir) => set({ destinationDir: destinationDir.replaceAll("\\", "/") }),

    unsortedFiles: [],
    setUnsortedFiles: (files) => set({ unsortedFiles: files }),
}));

export const useSortingStore = createSelectors(useSortingStoreBase);
