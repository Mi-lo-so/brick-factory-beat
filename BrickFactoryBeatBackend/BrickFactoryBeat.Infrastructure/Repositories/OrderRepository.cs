using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BrickFactoryBeat.Infrastructure.Repositories;

public class OrderRepository(AppDbContext db)  : IOrderRepository
{
    public async Task AddAsync(Order order)
    {
        if (order.Id.IsNullOrEmpty())
        {
            order.Id = Guid.NewGuid().ToString(); // ensure Id is set
        }

        if (order.StartedAt == default)
        {
            order.StartedAt = DateTime.UtcNow; 
        }

        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
        return db.Orders
            .FirstOrDefault(o => o.Id == id);
    }

    public async Task<List<Order>?> GetAllAsync(string equipmentId)
    {
        return await db.Orders
            .Where(o => o.EquipmentId == equipmentId)
            .ToListAsync();
    }
}