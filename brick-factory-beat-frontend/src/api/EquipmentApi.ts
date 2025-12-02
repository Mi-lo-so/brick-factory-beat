import axios from "axios";

export interface EquipmentTask {
    type: string;
    duration: number;
}

export interface Order {
    id: string;
    title: string;
    orderType: string; // can be a type
    equipmentId: string;
    status: string;
    startedAt: Date;
    completedAt?: Date;
}

export interface StateHistoryRecord {
    id: string;
    equipmentId: string;
    oldState: EquipmentState;
    state: EquipmentState;
    order?: Order;
    orderId?: string;
    changedAt: Date;
}

export enum EquipmentType {
    MaterialMelter,
    BrickMold,
    PaintBooth,
    QualityTester
}
const typeLabels = ["MaterialMelter",
    "BrickMold",
    "PaintBooth",
    "QualityTester"];
export const getEquipmentTypeLabel = (state: number) => typeLabels[state] ?? "Unknown";

export enum EquipmentState {
    Red = 0,
    Yellow = 1,
    Green = 2,
}
const stateLabels = ["Red", "Yellow", "Green"];
export const getStateLabel = (state: number) => stateLabels[state] ?? "Unknown";

export interface Equipment {
    id: string;
    name: string;
    type: EquipmentType; // can be a type
    state: EquipmentState; // can be a type
    orders?: Order[];
    stateHistory?: StateHistoryRecord[];
}

export enum OrderType
{
    SlowMold,
    FastMold,
    GradientColorMold,
    SolidColorMold,
    HeatMaterial,
}

// API base URL
const API_BASE = "https://localhost:7097";

export const getAllEquipments = async (): Promise<Equipment[]> => {
    try {
        const response = await axios.get<Equipment[]>(`${API_BASE}/Equipment`);
        return response.data;
    }
    catch (err) {
        console.error("Error fetching equipment list from " + API_BASE+"/Equipment", err);
        return []
    }
};


export const getEquipmentById = async (id: string): Promise<Equipment> => {
    const response = await axios.get<Equipment>(`${API_BASE}/Equipment/${id}`);
    return response.data;
};

export const createEquipment = async (name: string, type: EquipmentType): Promise<Equipment> => {
    try {
    const response = await axios.post<Equipment>(`${API_BASE}/Equipment/Create`, { Name: name, Type: 1})//type });
    return response.data;
    } catch (err) {
        console.error("Error creating equipment on " + API_BASE+"/Equipment/Create", err);
        throw err;
    }
};

export const updateEquipmentState = async (
    id: string,
    state: EquipmentState
): Promise<Equipment> => {
    console.log("Updating equipment state", id, state);
    const response = await axios.post<Equipment>(`${API_BASE}/Equipment/${id}/state`, { state });
    return response.data;
};

export const createOrder = async (id: string, title: string, orderType: OrderType): Promise<Order> => {
    const order ={
            EquipmentId: id,
            Title: title,
            OrderType: orderType,
            Status: "Pending",
            StartedAt: new Date().toISOString()
    };

    const response = await axios.post<Order>(`${API_BASE}/Equipment/${id}/order`, { order });
    return response.data;
};

export const startOrder = async (equipmentId:string, id: string): Promise<void> => {
    const response = await axios.post<Order>(`${API_BASE}/Equipment/${equipmentId}/order/${id}/start`);
};