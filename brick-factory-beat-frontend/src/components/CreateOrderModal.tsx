import React, { useState } from "react";
import {createOrder, OrderType} from "../api/EquipmentApi";

interface Props {
    equipmentId: string;
    onClose: () => void;
    onCreated: () => void;
}

const CreateOrderModal: React.FC<Props> = ({ equipmentId, onClose, onCreated }) => {
    const [title, setTitle] = useState("");
    const [type, setType] = useState<OrderType>(OrderType.FastMold);
    const [loading, setLoading] = useState(false);

    const handleSubmit = async () => {
        setLoading(true);
        try {
            await createOrder(equipmentId, title, type);
            onCreated(); // refresh equipment/orders
            onClose();
        } catch (err) {
            console.error(err);
            alert("Failed to create order");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div
            onClick={(e) => {
                e.stopPropagation();
            }}
            style={{
                position: "fixed",
                top: 0, left: 0,
                width: "100%", height: "100%",
                backgroundColor: "rgba(0,0,0,0.6)",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                zIndex: 1000,
            }}
        >
            <div style={{ backgroundColor: "#1e1e1e", padding: "1rem", borderRadius: "8px", minWidth: "300px" }}>
                <h3>Create Order</h3>
                <input
                    type="text"
                    placeholder="Order title"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    style={{ width: "100%", padding: "0.5rem", marginBottom: "0.5rem", backgroundColor: "#333", color: "#fff", border: "1px solid #555" }}
                />
                <select
                    value={type}
                    onChange={(e) => setType(Number(e.target.value) as OrderType)}
                    style={{ width: "100%", padding: "0.5rem", marginBottom: "0.5rem", backgroundColor: "#333", color: "#fff", border: "1px solid #555" }}
                >
                    {Object.keys(OrderType).filter(orderTypeKey => isNaN(Number(orderTypeKey))).map(k => (
                        <option key={k} value={OrderType[k as keyof typeof OrderType]}>
                            {k}
                        </option>
                    ))}
                </select>
                <div style={{ display: "flex", justifyContent: "flex-end", gap: "0.5rem" }}>
                    <button onClick={onClose} style={{ padding: "0.5rem", backgroundColor: "#555", color: "#fff" }}>Cancel</button>
                    <button onClick={handleSubmit} disabled={loading} style={{ padding: "0.5rem", backgroundColor: "#0a84ff", color: "#fff" }}>
                        {loading ? "Creating..." : "Create"}
                    </button>
                </div>
            </div>
        </div>
    );
};

export default CreateOrderModal;
