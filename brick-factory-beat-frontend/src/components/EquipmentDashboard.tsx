import React, {useState} from "react";
import CreateEquipmentForm from "./CreateEquipmentForm";
import EquipmentList from "./EquipmentList";


const EquipmentDashboard: React.FC = () => {
    const [refresh, setRefresh] = useState(false);
    const triggerRefresh = () => setRefresh(!refresh);


    return <div style={{padding: "2rem"}}>
        <h1 style={{color: "cornflowerblue"}}>Brick Factory Beats</h1>

        <div style={{ fontSize: "1.5rem", marginRight: "1rem"}}>Add a new piece of equipment:</div>
        <CreateEquipmentForm onCreated={triggerRefresh}/>

        <br/>

        <EquipmentList key={refresh ? "r1" : "r0"}/>
    </div>
}
export default EquipmentDashboard;