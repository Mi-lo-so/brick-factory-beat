using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Domain.StateHistory;

namespace BrickFactoryBeat.Application.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IStateHistoryRepository _stateHistoryRepository;

    public EquipmentService(
        IEquipmentRepository equipmentRepository,
        IOrderRepository orderRepository,
        IStateHistoryRepository stateHistoryRepository)
    {
        _equipmentRepository = equipmentRepository;
        _orderRepository = orderRepository;
        _stateHistoryRepository = stateHistoryRepository;
    }

    public async Task<Equipment> CreateEquipmentAsync(string name, EquipmentType type)
    {
        var equipment = new Equipment
        {
            Id = Guid.NewGuid().ToString(),
            Name = name, 
            State = EquipmentState.Red,
            Type = type
        };

        await _equipmentRepository.AddAsync(equipment);
        return equipment;
    }

    public async Task DeleteEquipmentAsync(string equipmentId)
    {
        await _equipmentRepository.DeleteAsync(equipmentId);
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(string equipmentId) =>
        await _equipmentRepository.GetByIdAsync(equipmentId);

    public async Task<Equipment?> GetEquipmentByNameAsync(string name) =>
        await _equipmentRepository.GetByNameAsync(name);

    public async Task<List<Equipment>> GetAllEquipmentAsync() =>
        await _equipmentRepository.GetAllAsync();


    /// <summary>
    /// Update the state of the equipment, creates a new state history object,
    /// and adds current order to the state history record.
    /// </summary>
    /// <param name="equipmentId"></param>
    /// <param name="newState"></param>
    /// <param name="orderId"></param>
    /// <exception cref="Exception"></exception>
    public async Task UpdateStateAsync(string equipmentId, EquipmentState newState, string? orderId = null)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(equipmentId)
            ?? throw new Exception($"Equipment {equipmentId} not found.");
        
        var oldState = equipment.State;
        equipment.State = newState;
        
        Order? order = null;
        orderId ??= equipment.Orders.FirstOrDefault()?.Id;

        if (!string.IsNullOrEmpty(orderId))
        {
            // Load the order entity so EF can track it
            order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception($"Order {orderId} not found.");
        }

        // Persist state change
        var history = new StateHistoryRecord
        {
            EquipmentId = equipmentId,
            OrderId = orderId,
            Order = order,
            OldState = oldState,
            State = newState,
            ChangedAt = DateTime.UtcNow
        };

        await _stateHistoryRepository.AddAsync(history);
        await _equipmentRepository.UpdateAsync(equipment);
    }


    /// <summary>
    /// Consider checking if order is exists, and changing its EquipmentId if needed,
    /// and only creating a new order for the machine if not found.
    /// </summary>
    /// <param name="equipmentId"></param>
    /// <param name="order"></param>
    /// <exception cref="Exception"></exception>
    public async Task AddOrderToEquipmentAsync(string equipmentId, Order order)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(equipmentId)
            ?? throw new Exception($"Equipment {equipmentId} not found.");
        
        //order.Equipment = equipment;
        

        try
        {
            await _orderRepository.AddAsync(order);
        } catch (Exception ex)
        {
            throw new Exception($"Failed to add order {order.Id} to equipment {equipmentId}: {ex.Message}");
        }
    }

    public async Task<List<Order>?> GetAllOrdersForEquipmentAsync(string equipmentId)
    {
        return await _orderRepository.GetAllAsync(equipmentId);
    }

    public async Task<List<StateHistoryRecord>?> GetHistoryForEquipmentAsync(string equipmentId)
    {
        return await _stateHistoryRepository.GetByEquipmentIdAsync(equipmentId);
    }
}
