namespace BrickFactoryBeat.Domain.StateHistory;

public interface IStateHistoryRepository
{
    Task AddAsync(StateHistoryRecord entry);
    Task<List<StateHistoryRecord>?> GetByEquipmentIdAsync(string equipmentId);
}