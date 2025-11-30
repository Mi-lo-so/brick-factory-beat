import React, { useState } from "react";
import EquipmentList from "./components/EquipmentList";
import CreateEquipmentForm from "./components/CreateEquipmentForm";
import {createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import EquipmentDashboard from "./components/EquipmentDashboard";
import EquipmentDetailPage from "./components/Detail/EquipmentDetail";

const darkTheme = createTheme({
    palette: {
        mode: "dark",
        primary: {
            main: "#90caf9",
        },
        secondary: {
            main: "#f48fb1",
        },
        background: {
            default: "#121212",
            paper: "#1e1e1e",
        },
    },
});

const App: React.FC = () => {

    return (
        <ThemeProvider theme={darkTheme}>

            <CssBaseline />
            <Router>
                <Routes>
                    <Route path="/" element={<EquipmentDashboard />} />
                    <Route path="/equipment/:id" element={<EquipmentDetailPage />} />
                </Routes>
            </Router>

        </ThemeProvider>
    );
};

export default App;
