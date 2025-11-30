import React, { useEffect, useState } from "react";
import {
    Equipment,
    EquipmentState,
    getAllEquipments,
    getEquipmentById, getEquipmentTypeLabel,
    StateHistoryRecord
} from "../../api/EquipmentApi";
import { useParams, Link } from "react-router-dom";
import {ChangeStateButton} from "../ChangeStateButton";
import {typeIcons} from "../stateIcons";

const stateLabels: Record<EquipmentState, string> = {
    [EquipmentState.Red]: "Red",
    [EquipmentState.Yellow]: "Yellow",
    [EquipmentState.Green]: "Green",
};

const stateIcons: Record<EquipmentState, string> = {
    [EquipmentState.Red]: "🔴",
    [EquipmentState.Yellow]: "🟡",
    [EquipmentState.Green]: "🟢",
};

const EquipmentDetailPage: React.FC = () => {
    const { id } = useParams<{ id: string }>();
    const [equipment, setEquipment] = useState<Equipment | null>(null);
    const [loading, setLoading] = useState(true);

    const fetchEquipment = async () => {
        const loadEquipmentById = async () => {
            if (!id) return;
            try {
                const data = await getEquipmentById(id);
                setEquipment(data);
            } catch (err) {
                console.error("Failed to load equipment", err);
            } finally {
                setLoading(false);
            }
        };
        await loadEquipmentById();
    };

    useEffect(() => {
        fetchEquipment();
    }, [id]);

    if (loading) return <div style={{ padding: "1rem" }}>Loading...</div>;
    if (!equipment) return <div style={{ padding: "1rem" }}>Equipment not found</div>;

    // Sort history by ChangedAt descending (latest first)
    const sortedHistory = equipment.stateHistory ? [...equipment.stateHistory].sort(
        (a, b) => new Date(a.order?.startedAt ?? new Date()).getTime() - new Date(b.order?.startedAt?? new Date()).getTime()
    ) : [];

    return (
        <div style={{ padding: "1rem", color: "#e0e0e0" }}>
            <Link to="/">← Back to overview</Link>

            <h1 style={{ margin: "0.5rem" }}>Equipment Name: {equipment.name}</h1>
            <div style={{ fontSize: "3rem", marginBottom: "1rem" }} aria-label={getEquipmentTypeLabel(equipment.type)}>
                {typeIcons[equipment.type] ?? typeIcons.Default}
                <span  style={{ fontSize: "1rem", marginRight: "1rem"}}>Equipment Type: {getEquipmentTypeLabel(equipment.type)}</span>
            </div>

            <div style={{ marginBottom: "2rem" }}>
                <strong>Current state:</strong>{" "}
                {stateIcons[equipment.state]} {stateLabels[equipment.state]}
                <div style={{ color: "#555", marginRight: "2rem", }}>
                    <ChangeStateButton onUpdated={fetchEquipment} equipment={equipment} />
                </div>
            </div>

            <EquipmentOrders equipment={equipment} />

            <StateHistory sortedHistory={sortedHistory} />
        </div>
    );
};

interface EquipmentOrdersProps {
    equipment: Equipment;
}

const EquipmentOrders: React.FC<EquipmentOrdersProps> = ({equipment}:EquipmentOrdersProps) => {
    console.log(equipment);
    return <section style={{ marginBottom: "1.5rem" }}>
        <h2>Orders</h2>
        {equipment.orders ? equipment.orders.length === 0 && <div>No orders yet.</div> : null}

        {equipment.orders ? equipment.orders.map((o) => (
            <div
                key={o.id}
                style={{
                    border: "1px solid #333",
                    padding: "0.5rem",
                    marginBottom: "0.5rem",
                    borderRadius: "0.5rem",
                    backgroundColor: "#222",
                }}
            >
                <div>
                    <strong>{o.title}</strong> ({o.status})
                </div>
                <div style={{ fontSize: "0.85rem", color: "#ccc" }}>
                    Started: {new Date(o.startedAt).toLocaleString()}
                    { <>   --|--   Completed: {o.CompletedAt ? new Date(o.CompletedAt).toLocaleString() : "N/A"}</>}
                </div>
            </div>
        )) : null}
    </section>
}

interface StateHistoryProps  {
    sortedHistory: StateHistoryRecord[];
}

const StateHistory: React.FC<StateHistoryProps> = ({sortedHistory}:StateHistoryProps) => {
    return <section>
        <h2>State History</h2>

        {sortedHistory.length === 0 && <div>No state changes recorded yet.</div>}

        {sortedHistory.map((h: StateHistoryRecord) => (
            <div
                key={h.id}
                style={{
                    borderLeft: "4px solid #555",
                    padding: "0.5rem 0.5rem 0.5rem 0.75rem",
                    marginBottom: "0.5rem",
                    backgroundColor: "#1a1a1a",
                    borderRadius: "0.25rem",
                }}
            >
                <div style={{ fontSize: "0.85rem", color: "#aaa" }}>
                    {h.order?.startedAt ? new Date(h.order?.startedAt).toLocaleString(): null}
                </div>
                <div>
                    {stateIcons[h.oldState]} {stateLabels[h.oldState]} →{" "}
                    {stateIcons[h.state]} {stateLabels[h.state]}
                </div>
                {h.order && (
                    <div style={{ fontSize: "0.85rem", color: "#ccc" }}>
                        During order: <strong>{h.order.title}</strong> ({h.order.status})
                    </div>
                )}
            </div>
        ))}
    </section>
}

export default EquipmentDetailPage;
