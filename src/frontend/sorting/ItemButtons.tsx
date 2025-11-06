import { Button, Stack } from "@mui/material";
import { exifToolService } from "../../services/ExifToolService";
import { useCurrentFileStore } from "../../stores/CurrentFileStore";
import { useSortingStore } from "../../stores/SortingStore";

export default function ItemButtons() {

    const currentFile = useCurrentFileStore.use.currentFile()
    const newFilename = useCurrentFileStore.use.newFilename()
    const destinationDir = useSortingStore.use.destinationDir()
    const getDestinationPath = useCurrentFileStore.use.getDestinationPath()


    async function onNextClick() {
        await backend.moveFile(currentFile!.filePath, process.cwd() + getDestinationPath())
        await exifToolService.writeDateTaken()
    }

    async function onDeleteClick() {

    }

    async function onSkipClick() {

    }

    return (
        <Stack gap={0.5}>
            <Button onClick={onNextClick} variant="outlined">Next</Button>
            <Button onClick={onDeleteClick} variant="outlined">Delete</Button>
            <Button onClick={onSkipClick} variant="outlined">Skip</Button>
        </Stack>
    )
}