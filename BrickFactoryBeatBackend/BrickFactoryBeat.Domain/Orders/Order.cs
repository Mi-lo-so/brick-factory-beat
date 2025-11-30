using System.Text.Json.Serialization;
using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.StateHistory;

namespace BrickFactoryBeat.Domain.Orders;

/// <summary>
/// An order for equipment to execute.
/// If we assume there are multiple types of equipment, there should be different order for each,
/// but for simplicity, I will treat them all as one.
/// </summary>
public class Order
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public OrderType OrderType { get; set; }
    
    public string EquipmentId { get; set; }
    
    [JsonIgnore]
    public Equipment.Equipment Equipment { get; set; }
    
    // status needed? e.g. completed, in-progress, pending
    public string Status { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

}