import React, { useEffect, useState } from "react";
import {getAllEquipments, Equipment, getStateLabel, getEquipmentTypeLabel, Order} from "../api/EquipmentApi";
import EquipmentRow from "./EquipmentRow";

const EquipmentList: React.FC = () => {
    const [equipment, setEquipment] = useState<Equipment[]>([]);
    const [loading, setLoading] = useState(true);

    const fetchEquipment = async () => {
        setLoading(true);
        try {
            const data = await getAllEquipments();
            setEquipment(data);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchEquipment();
    }, []);

    if (loading) return <div>Loading...</div>;

    console.log(equipment);

    return (
        <div>
            {equipment.map((eq) => (
                <EquipmentRow key={eq.id} equipment={eq} onUpdated={fetchEquipment} />
            ))}
        </div>
    );


};


export default EquipmentList;
