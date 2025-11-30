using BrickFactoryBeat.Domain.StateHistory;
using BrickFactoryBeat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BrickFactoryBeat.Infrastructure.Repositories;

public class StateHistoryRepository(AppDbContext db) : IStateHistoryRepository
{
    public async Task AddAsync(StateHistoryRecord entry)
    {
        if (entry.Id == Guid.Empty)
        {
            entry.Id = Guid.NewGuid(); // ensure Id is set
        }

        if (entry.ChangedAt == default)
        {
            entry.ChangedAt = DateTime.UtcNow; // set timestamp
        }

        db.StateHistory.Add(entry);
        await db.SaveChangesAsync();
    }

    public async Task<List<StateHistoryRecord>?> GetByEquipmentIdAsync(string equipmentId)
    {
        return await db.StateHistory.Where(o => o.EquipmentId == equipmentId)
            .Include(o => o.Order)
            .ToListAsync();
    }
}