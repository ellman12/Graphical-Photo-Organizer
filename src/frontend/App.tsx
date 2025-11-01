import { Box } from "@mui/material";
import { useAppStateStore } from "../stores/AppStateStore";
import SortingSetup from "./setup/SortingSetup";

export default function App() {
    const appMode = useAppStateStore.use.appMode();

    return <Box className="h-screen w-screen bg-black p-2">{appMode === "preSort" && <SortingSetup />}</Box>;
}
