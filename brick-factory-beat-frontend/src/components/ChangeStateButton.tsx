import React, {useState} from "react";
import {Equipment, EquipmentState, getStateLabel, updateEquipmentState} from "../api/EquipmentApi";
import {stateIcons} from "./stateIcons";
import {getNextState} from "./commonUtils";

interface Props {
    equipment: Equipment;
    onUpdated: () => void; // refreshes list
}



export const ChangeStateButton: React.FC<Props> = ({ equipment, onUpdated}) => {
    const [stateLoading, setStateLoading] = useState(false);
    const [menuOpen, setMenuOpen] = useState(false);

    // Compute allowed transitions
    const getAllowedStates = (): EquipmentState[] => {
        switch (equipment.state) {
            case EquipmentState.Red:
            case EquipmentState.Green:
                return [EquipmentState.Yellow]; // only Yellow allowed
            case EquipmentState.Yellow:
                return [EquipmentState.Red, EquipmentState.Green]; // can go to Red or Green
            default:
                return [];
        }
    }

    const handleStateSelect = async (newState: EquipmentState) => {
        setStateLoading(true);
        try {
            await updateEquipmentState(equipment.id, newState);
            onUpdated(); // refresh list
        } catch (err) {
            console.error("Failed to update state", err);
        } finally {
            setStateLoading(false);
            setMenuOpen(false);
        }
    };

    return (
        <div style={{ position: "relative", display: "inline-block" }}>
            <button
                onClick={(e) => {
                    e.stopPropagation(); // Prevent row click from list row
                    setMenuOpen(!menuOpen)}}
                disabled={stateLoading}
                style={{
                    backgroundColor: "#333",
                    color: "#fff",
                    border: "1px solid #555",
                    padding: "0.25rem 0.5rem",
                    borderRadius: "6px",
                    cursor: "pointer",
                }}
            >
                {stateLoading
                    ? "Updating..."
                    : `Set state ${stateIcons[equipment.state]}`}
            </button>

            {menuOpen && (
                <div
                    style={{
                        position: "absolute",
                        top: "100%",
                        left: 0,
                        backgroundColor: "#222",
                        border: "1px solid #555",
                        borderRadius: "6px",
                        marginTop: "0.25rem",
                        zIndex: 10,
                        minWidth: "120px",
                    }}
                >
                    {getAllowedStates().map((s) => (
                        <div
                            key={s}
                            onClick={(e) => {
                                e.stopPropagation(); // Prevent row click from list row
                                handleStateSelect(s);}}
                            style={{
                                padding: "0.25rem 0.5rem",
                                cursor: "pointer",
                                display: "flex",
                                alignItems: "center",
                                gap: "0.5rem",
                                color: "#fff",
                            }}
                        >
                            <span>{stateIcons[s]}</span>
                            <span>{getStateLabel(s)}</span>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}