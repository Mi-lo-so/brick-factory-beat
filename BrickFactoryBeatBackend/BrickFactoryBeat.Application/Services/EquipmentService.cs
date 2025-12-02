using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Domain.StateHistory;
using Microsoft.Extensions.DependencyInjection;

namespace BrickFactoryBeat.Application.Services;

public class EquipmentService(
    IEquipmentRepository equipmentRepository,
    IOrderRepository orderRepository,
    IStateHistoryRepository stateHistoryRepository,
    IServiceScopeFactory _scopeFactory
  )
    : IEquipmentService
{
    public async Task<Equipment> CreateEquipmentAsync(string name, EquipmentType type)
    {
        var equipment = new Equipment
        {
            Id = Guid.NewGuid().ToString(),
            Name = name, 
            State = EquipmentState.Red,
            Type = type
        };

        await equipmentRepository.AddAsync(equipment);
        return equipment;
    }

    public async Task DeleteEquipmentAsync(string equipmentId)
    {
        await equipmentRepository.DeleteAsync(equipmentId);
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(string equipmentId) =>
        await equipmentRepository.GetByIdAsync(equipmentId);

    public async Task<Equipment?> GetEquipmentByNameAsync(string name) =>
        await equipmentRepository.GetByNameAsync(name);

    public async Task<List<Equipment>> GetAllEquipmentAsync() =>
        await equipmentRepository.GetAllAsync();


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
        var equipment = await equipmentRepository.GetByIdAsync(equipmentId)
            ?? throw new Exception($"Equipment {equipmentId} not found.");
        
        var oldState = equipment.State;
        equipment.State = newState;
        
        Order? order = null;
        // If no orderId is provided, use the current order or the first order in the scheduled
        orderId ??= equipment.CurrentOrder != null ? equipment.CurrentOrder.Id : equipment.Orders.FirstOrDefault()?.Id;
        

        if (!string.IsNullOrEmpty(orderId))
        {
            // Load the order entity so EF can track it
            order =  await orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception($"Order {orderId} not found.");

            order.Status = newState switch
            {
                // Update order status based on equipment state
                EquipmentState.Green => "Running",
                EquipmentState.Red => "Stopped",
                EquipmentState.Yellow => "Paused",
                _ => order.Status
            };
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

        await stateHistoryRepository.AddAsync(history);
        await equipmentRepository.UpdateAsync(equipment);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="equipmentId"></param>
    /// <param name="order"></param>
    /// <exception cref="Exception"></exception>
    public async Task AddOrderToEquipmentAsync(string equipmentId, Order order)
    {
        var equipment = await equipmentRepository.GetByIdAsync(equipmentId)
            ?? throw new Exception($"Equipment {equipmentId} not found.");
        
        //order.Equipment = equipment;
        

        try
        {
            await orderRepository.AddAsync(order);
        } catch (Exception ex)
        {
            throw new Exception($"Failed to add order {order.Id} to equipment {equipmentId}: {ex.Message}");
        }
    }

    public async Task<List<Order>?> GetAllOrdersForEquipmentAsync(string equipmentId)
    {
        return await orderRepository.GetAllAsync(equipmentId);
    }

    public async Task<List<StateHistoryRecord>?> GetHistoryForEquipmentAsync(string equipmentId)
    {
        return await stateHistoryRepository.GetByEquipmentIdAsync(equipmentId);
    }
    
    public async Task StartNextOrderAsync(string equipmentId, string? orderId = null)
    {
        var equipment = await equipmentRepository.GetByIdAsync(equipmentId)
                        ?? throw new Exception($"Equipment {equipmentId} not found.");
        
        if (equipment.State != EquipmentState.Green)
        {
            throw new Exception($"Cannot start order on equipment {equipmentId} because it is not in Green state.");
        }
        
        Order? order = null;
        // If no orderId is provided, use the current order or the first order in the scheduled
        orderId ??= equipment.CurrentOrder != null ? equipment.CurrentOrder.Id : equipment.Orders.FirstOrDefault()?.Id;
        
        if (string.IsNullOrEmpty(orderId))
        {
            return;
        }
       
        order =  await orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception($"Order {orderId} not found.");
        
        order.Status = "Completed";
        order.CompletedAt = DateTime.UtcNow;

        await orderRepository.UpdateAsync(order);
        
        var nextOrder = equipment.Orders
            .Where(o => o.Status =="Pending")
            .OrderBy(o => o.Duration)
            .FirstOrDefault();
        equipment.CurrentOrder = nextOrder;
        await equipmentRepository.UpdateAsync(equipment);
    }


}
