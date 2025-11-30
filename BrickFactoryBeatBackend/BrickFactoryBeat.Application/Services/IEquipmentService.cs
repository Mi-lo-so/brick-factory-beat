using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Domain.StateHistory;

namespace BrickFactoryBeat.Application.Services;

public interface IEquipmentService
{
    Task<Equipment> CreateEquipmentAsync(string name, EquipmentType type);
    Task DeleteEquipmentAsync(string equipmentId);
    Task<Equipment?> GetEquipmentByIdAsync(string equipmentId);
    Task<Equipment?> GetEquipmentByNameAsync(string name);
    Task<List<Equipment>> GetAllEquipmentAsync();

    Task UpdateStateAsync(string equipmentId, EquipmentState newState, string? orderId = null);
    Task AddOrderToEquipmentAsync(string equipmentId, Order order);
    
    Task<List<Order>?> GetAllOrdersForEquipmentAsync(string equipmentId);
    
    Task<List<StateHistoryRecord>?> GetHistoryForEquipmentAsync(string equipmentId);
}