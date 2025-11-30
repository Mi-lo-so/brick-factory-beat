using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Domain.StateHistory;

namespace BrickFactoryBeat.Domain.Equipment;

/// <summary>
/// Basic class for equipment type. It is unclear what the equipment can do,
/// so I will treat them all the same for now.
/// </summary>
public class Equipment
{
    public string Name { get; set; } = string.Empty;
    public required string Id { get; set; }
    public EquipmentState State { get; set; }
    public EquipmentType Type { get; set; }
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<StateHistoryRecord> StateHistory { get; set; } = new List<StateHistoryRecord>();
}