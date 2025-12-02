namespace BrickFactoryBeat.Domain.Orders;

public interface IOrderRepository
{
    Task UpdateAsync(Order order);
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(string id);
    Task<List<Order>?> GetAllAsync(string equipmentId);
}