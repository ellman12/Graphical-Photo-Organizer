import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import { ThemeProvider, createTheme } from "@mui/material";
import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./App.css";
import DevHelper from "./DevHelper";

const theme = createTheme({
    typography: {
        allVariants: {
            color: "white",
            fontFamily: "Roboto",
        },
    },
});

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
    <React.StrictMode>
        <ThemeProvider theme={theme}>
            {import.meta.env.MODE === "development" && <DevHelper />}
            <App />
        </ThemeProvider>
    </React.StrictMode>
);
