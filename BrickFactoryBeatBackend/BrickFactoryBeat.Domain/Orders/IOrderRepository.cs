namespace BrickFactoryBeat.Domain.Orders;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(string id);
    Task<List<Order>?> GetAllAsync(string equipmentId);
}