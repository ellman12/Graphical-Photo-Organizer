import { Button, Stack, Typography } from "@mui/material";
import { useAppStateStore } from "../../stores/AppStateStore";
import { useCurrentFileStore } from "../../stores/CurrentFileStore";
import { useSortingStore } from "../../stores/SortingStore";

type SetupFormFields = {
    source?: string;
    destination?: string;
};

export default function SortingSetup() {
    const setAppMode = useAppStateStore.use.setAppMode();
    const { sourceDir, setSourceDir, destinationDir, setDestinationDir, unsortedFiles, setUnsortedFiles } = useSortingStore();
    const setCurrentFile = useCurrentFileStore.use.setCurrentFile();

    async function pickSourceDir() {
        const path = (await backend.pickFolder()) ?? sourceDir;
        setSourceDir(path);

        const files = await backend.getUnsortedFiles(path);
        setUnsortedFiles(files);
        setCurrentFile(files[0]);
    }

    async function pickDestinationDir() {
        setDestinationDir((await backend.pickFolder()) ?? destinationDir);
    }

    function getInputErrors() {
        const newErrors: SetupFormFields = {};

        if (sourceDir !== "" && destinationDir !== "" && sourceDir === destinationDir) {
            newErrors.source = newErrors.destination = "Source and destination cannot be the same";
        }

        if (sourceDir !== "" && unsortedFiles.length === 0) {
            newErrors.source = "No supported files found";
        }

        return newErrors;
    }

    const errors = getInputErrors();
    const startButtonEnabled = sourceDir !== "" && destinationDir !== "" && sourceDir !== destinationDir && !Object.values(errors).some((e) => e !== undefined);

    function startSorting() {
        setAppMode("sorting");
    }

    return (
        <Stack gap={2}>
            <Stack gap={0.5}>
                <Button onClick={pickSourceDir} variant="outlined">
                    Choose Source Folder
                </Button>

                <Typography color={errors.source ? "error" : ""}>{sourceDir}</Typography>
                {!errors.source && unsortedFiles.length > 0 && <Typography>{unsortedFiles.length} Unsorted Files</Typography>}
                {errors.source && <Typography color="error">{errors.source}</Typography>}
            </Stack>

            <Stack gap={0.5}>
                <Button onClick={pickDestinationDir} variant="outlined">
                    Choose Destination Folder
                </Button>

                <Typography color={errors.destination ? "error" : ""}>{destinationDir}</Typography>
                {errors.destination && <Typography color="error">{errors.source}</Typography>}
            </Stack>

            <Button onClick={startSorting} variant="outlined" disabled={!startButtonEnabled}>
                Begin Sorting
            </Button>
        </Stack>
    );
}
