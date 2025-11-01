import { useEffect } from "react";
import { useSortingStore } from "../stores/SortingStore";

//Automates annoying tasks when working in dev.
export default function DevHelper() {
    const setSourceDir = useSortingStore.use.setSourceDir();
    const setDestinationDir = useSortingStore.use.setDestinationDir();
    const setUnsortedPaths = useSortingStore.use.setUnsortedFiles();

    useEffect(() => {
        setSourceDir(import.meta.env.VITE_GPO_SOURCE_DIR);
        setDestinationDir(import.meta.env.VITE_GPO_DESTINATION_DIR);

        backend.getUnsortedFiles(import.meta.env.VITE_GPO_SOURCE_DIR).then((e) => setUnsortedPaths(e));
    }, []);

    return null;
}
