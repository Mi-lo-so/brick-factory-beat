import React, { useState } from "react";
import {createEquipment, EquipmentType} from "../api/EquipmentApi";

interface Props {
    onCreated: () => void; //  refreshes list
}

const CreateEquipmentForm: React.FC<Props> = ({ onCreated }) => {
    const [name, setName] = useState("");
    const [type, setType] = useState<EquipmentType>(EquipmentType.BrickMold);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await createEquipment(name, type);
            setName(""); // if successful, clear the form
            setType(EquipmentType.BrickMold);
            onCreated();
        } catch (err) {
            console.error("Error creating equipment", err);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Name:
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    required
                    style={{ padding: "0.5rem" }}
                />
            </label>

            <label>
                Type:
                <select
                    value={type}
                    onChange={(e) => setType(Number(e.target.value) as EquipmentType)}
                    style={{ padding: "0.5rem", color:"whitesmoke", backgroundColor:"#1e1e1e" }}
                >
                    {Object.keys(EquipmentType)
                        .filter((key) => isNaN(Number(key))) // only get enum names
                        .map((key) => (
                            <option key={key} value={EquipmentType[key as keyof typeof EquipmentType]}>
                                {key}
                            </option>
                        ))}
                </select>
            </label>
            <button type="submit">Add Equipment</button>
        </form>
    );
};

export default CreateEquipmentForm;
