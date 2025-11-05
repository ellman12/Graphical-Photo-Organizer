import { useEffect } from "react";
import { useCurrentFileStore } from "../stores/CurrentFileStore";
import { useSortingStore } from "../stores/SortingStore";

//Automates annoying tasks when working in dev.
export default function DevHelper() {
    const setSourceDir = useSortingStore.use.setSourceDir();
    const setDestinationDir = useSortingStore.use.setDestinationDir();
    const setUnsortedPaths = useSortingStore.use.setUnsortedFiles();
    const setCurrentFile = useCurrentFileStore.use.setCurrentFile();

    useEffect(() => {
        setSourceDir(import.meta.env.VITE_GPO_SOURCE_DIR);
        setDestinationDir(import.meta.env.VITE_GPO_DESTINATION_DIR);

        backend.getUnsortedFiles(import.meta.env.VITE_GPO_SOURCE_DIR).then((files) => {
            setUnsortedPaths(files);
            setCurrentFile(files[0]);
        });
    }, []);

    return null;
}
