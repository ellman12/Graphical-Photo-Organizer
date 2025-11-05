import { Stack } from "@mui/material";
import ItemControls from "./ItemControls";
import ItemDisplay from "./ItemDisplay";

export default function Sorting() {
    return (
        <Stack direction="row" spacing={2} className="size-full justify-between">
            <ItemControls />
            <ItemDisplay />
        </Stack>
    );
}
