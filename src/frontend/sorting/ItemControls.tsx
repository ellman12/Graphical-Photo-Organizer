import { Stack, TextField, Typography } from "@mui/material";
import { useCurrentFileStore } from "../../stores/CurrentFileStore";
import DateTakenSourcePicker from "./DateTakenSourcePicker";
import ItemButtons from "./ItemButtons";

export default function ItemControls() {
    const currentFile = useCurrentFileStore.use.currentFile();
    const newFilename = useCurrentFileStore.use.newFilename();
    const setNewFilename = useCurrentFileStore.use.setNewFilename();

    if (!currentFile) return null;

    return (
        <Stack direction="column" spacing={4} className="h-screen min-w-1/3 p-2">
            <Typography>Current File</Typography>

            <TextField value={newFilename} label="New Filename" variant="outlined" onChange={(event) => setNewFilename(event.target.value)} />

            <DateTakenSourcePicker />

            <ItemButtons/>
        </Stack>
    );
}
