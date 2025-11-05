import { Box } from "@mui/material";
import { useCurrentFileStore } from "../../stores/CurrentFileStore";

export default function ItemDisplay() {
    const currentFile = useCurrentFileStore.use.currentFile();

    return (
        <Box>
            <img src={`file:///${currentFile?.filePath ?? ""}`} className="max-h-3/4 w-auto object-contain" alt="" />
        </Box>
    );
}
