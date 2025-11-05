import { Box } from "@mui/material";
import { useAppStateStore } from "../stores/AppStateStore";
import SortingSetup from "./setup/SortingSetup";
import Sorting from "./sorting/Sorting";

export default function App() {
    const appMode = useAppStateStore.use.appMode();

    return (
        <Box className="box-border h-screen w-screen overflow-hidden bg-black p-2">
            {appMode === "preSort" && <SortingSetup />}
            {appMode === "sorting" && <Sorting />}
        </Box>
    );
}
