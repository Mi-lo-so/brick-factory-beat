import {
    Equipment,
    EquipmentState,
    getEquipmentTypeLabel,
    getStateLabel,
    Order,
    updateEquipmentState
} from "../api/EquipmentApi";
import React, {useState} from "react";
import CreateOrderModal from "./CreateOrderModal";
import { useNavigate } from "react-router-dom";
import {stateIcons, typeIcons} from "./stateIcons";
import {ChangeStateButton} from "./ChangeStateButton";



interface Props {
    equipment: Equipment;
    onUpdated: () => void; // to refresh the equipment list
}

const EquipmentRow: React.FC<Props> = ({ equipment, onUpdated }) => {
    const navigate = useNavigate();
    const typeIcon = typeIcons[equipment.type] ?? typeIcons.Default;
    const stateIcon = stateIcons[equipment.state] ?? "⚪";

    const [orderModalOpen, setOrderModalOpen] = useState(false);

    // Get latest order startedAt
    const latestOrder = equipment.orders?.reduce((latest, order) => {
        return !latest || new Date(order.startedAt) > new Date(latest.startedAt)
            ? order
            : latest;
    }, undefined as Order | undefined);

    const handleClick = () => {
        navigate(`/equipment/${equipment.id}`);
    };

    // Could probably be improved with an actual row class
    return (
        <div
            onClick={handleClick}
            style={{
                display: "flex",
                alignItems: "center",
                border: "1px solid #333",
                padding: "0.5rem 1rem",
                marginBottom: "0.5rem",
                borderRadius: "0.5rem",
                boxShadow: "1px 1px 4px rgba(0,0,0,0.5)",
                backgroundColor: "#1e1e1e",
                color: "#e0e0e0",
            }}
            className="flex justify-end gap-2"
        >
            <div style={{ color: "#555", marginRight: "1rem", }}>
                <ChangeStateButton onUpdated={onUpdated} equipment={equipment} />
            </div>
            <div style={{ fontSize: "3rem", marginRight: "1rem" }} aria-label={getStateLabel(equipment.state)}>{stateIcon}</div>
            <div style={{ fontSize: "3rem", marginRight: "1rem",  width: "200px"  }} aria-label={getEquipmentTypeLabel(equipment.type)}>{typeIcon} <span  style={{ fontSize: "1rem", marginRight: "1rem"}}>{getEquipmentTypeLabel(equipment.type)}</span> </div>

            <div style={{ fontWeight: "bold", fontSize: "2rem",  width: "350px"  }}>{equipment.name}</div>

            <div style={{ flexGrow: 1 }} /* right the rest to the right*/ />

            <div style={{ fontSize: "0.9rem", color: "#555", marginRight: "1rem" }}>
                {latestOrder ? `Order started: ${new Date(latestOrder.startedAt).toLocaleString()}` : "No orders"}
            </div>

            <button onClick={(e) => {e.stopPropagation(); setOrderModalOpen(true);}}  style={{ padding: "0.25rem 0.5rem" }}>
                Add Order
            </button>


            {orderModalOpen && (
                <CreateOrderModal
                    equipmentId={equipment.id}
                    onClose={() => setOrderModalOpen(false)}
                    onCreated={onUpdated} // refreshes list
                />
            )}

        </div>
    );
};

export default EquipmentRow;