using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BrickFactoryBeat.Infrastructure.Repositories;

public class EquipmentRepository(AppDbContext db) : IEquipmentRepository
{
    public async Task AddAsync(Equipment equipment)
    {
        db.Equipment.Add(equipment);
        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string equipmentId)
    {
        var e = await db.Equipment.FindAsync(equipmentId);
        if (e != null)
        {
            db.Equipment.Remove(e);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        db.Equipment.Update(equipment);
        await db.SaveChangesAsync();
    }

    public async Task<Equipment?> GetByIdAsync(string id)
    {
        var equipment = await db.Equipment
            .Include(e => e.Orders)
            .Include(e => e.StateHistory)
            .ThenInclude(h => h.Order)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipment != null)
        {
            // Order newest first
            equipment.Orders = equipment.Orders
                .OrderByDescending(o => o.StartedAt)
                .ToList();

            equipment.StateHistory = equipment.StateHistory
                .OrderByDescending(h => h.ChangedAt)
                .ToList();
        }
        
        return equipment;
    }

    public async Task<Equipment?> GetByNameAsync(string name)
    {
        return await db.Equipment
            .FirstOrDefaultAsync(e => e.Name == name);
    }

    public async Task<List<Equipment>> GetAllAsync()
    {
        var x = await db.Equipment.Include(e => e.Orders)
            .ToListAsync();
        return x;
    }
}