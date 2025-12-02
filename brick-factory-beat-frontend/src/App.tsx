import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import EquipmentDashboard from "./components/EquipmentDashboard";
import EquipmentDetailPage from "./components/Detail/EquipmentDetail";

const App: React.FC = () => {

    return (
        <Router>
            <Routes>
                <Route path="/" element={<EquipmentDashboard />} />
                <Route path="/equipment/:id" element={<EquipmentDetailPage />} />
            </Routes>
        </Router>
    );
};

export default App;
