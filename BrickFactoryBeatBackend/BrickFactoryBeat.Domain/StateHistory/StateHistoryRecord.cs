using System.Text.Json.Serialization;
using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;

namespace BrickFactoryBeat.Domain.StateHistory;

public class StateHistoryRecord
{
    // do I need a StateHistoryType as well, or is it implied to only be for the state?
    public Guid Id { get; set; }
    public required string EquipmentId { get; set; }
    [JsonIgnore] //prevent circular reference on API serializing... or just make a DTO instead
    public Equipment.Equipment Equipment { get; set; }

    public string? OrderId { get; set; } // if an order was active, state it too.
    public Order? Order { get; set; }

    public EquipmentState OldState { get; set; } // reference last state. Could go by date, but that requires thinking.
    public EquipmentState State { get; set; }
    
    public DateTime ChangedAt { get; set; }
}