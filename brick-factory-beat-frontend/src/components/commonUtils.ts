import {EquipmentState} from "../api/EquipmentApi";

export const getNextState = (state: EquipmentState): EquipmentState => {
    // Assuming the point in the red green yellow, is that the winding up/down state is a middle state always
    switch (state) {
        case EquipmentState.Red:     // 0
            return EquipmentState.Yellow; // 1
        case EquipmentState.Yellow:  // 1
            return EquipmentState.Green;  // 2
        case EquipmentState.Green:   // 2
            return EquipmentState.Yellow; // 1
        default:
            return EquipmentState.Red;    // fallback
    }
};
