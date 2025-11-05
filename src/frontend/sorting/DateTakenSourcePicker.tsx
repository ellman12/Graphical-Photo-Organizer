import { MenuItem, TextField } from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { useState } from "react";
import { useCurrentFileStore } from "../../stores/CurrentFileStore";
import { DateTakenSource } from "../../types/DateTaken";

export default function DateTakenSourcePicker() {
    const currentFile = useCurrentFileStore.use.currentFile();
    const newDateTaken = useCurrentFileStore.use.newDateTaken();
    const setNewDateTaken = useCurrentFileStore.use.setNewDateTaken();
    const [dateTakenSource, setDateTakenSource] = useState<DateTakenSource>("Filename");

    if (!currentFile) return null;

    return (
        <>
            <TextField select label="Date Taken" variant="outlined" value={dateTakenSource} onChange={(e) => setDateTakenSource(e.target.value as DateTakenSource)}>
                {currentFile.dateTaken.filename && <MenuItem value="Filename">Filename: {currentFile.dateTaken.filename.toLocaleString()}</MenuItem>}

                {currentFile.dateTaken.metadata.map((m) => (
                    <MenuItem key={m.source} value={m.source}>
                        {m.source}: {m.value.toLocaleString()}
                    </MenuItem>
                ))}

                <MenuItem value="Custom">Custom</MenuItem>
                <MenuItem value="None">None</MenuItem>
            </TextField>

            {dateTakenSource === "Custom" && <DatePicker onChange={(e) => setNewDateTaken(e)} value={newDateTaken} label="New Date Taken" />}
        </>
    );
}
