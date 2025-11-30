namespace BrickFactoryBeat.Domain.Equipment;

public enum EquipmentState
{
    Red = 0, // standing still
    Yellow = 1, // winding down or starting up
    Green = 2, // Producing normally
}