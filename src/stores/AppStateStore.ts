import { create } from "zustand/react";
import { createSelectors } from "./createSelectors";

export type AppMode = "preSort" | "sorting";

//Stores app config.
interface AppStateStore {
    appMode: AppMode;
    startSorting: () => void;
}

const useAppStateStoreBase = create<AppStateStore>()((set) => ({
    appMode: "preSort",
    startSorting: () => set({ appMode: "sorting" }),
}));

export const useAppStateStore = createSelectors(useAppStateStoreBase);
