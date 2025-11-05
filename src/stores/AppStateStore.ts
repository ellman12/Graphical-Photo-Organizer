import { create } from "zustand/react";
import { createSelectors } from "./createSelectors";

export type AppMode = "preSort" | "sorting";

//Stores app config.
interface AppStateStore {
    appMode: AppMode;
    setAppMode: (newMode: AppMode) => void;
}

const useAppStateStoreBase = create<AppStateStore>()((set) => ({
    appMode: "preSort",
    setAppMode: (newMode) => set({ appMode: newMode }),
}));

export const useAppStateStore = createSelectors(useAppStateStoreBase);
