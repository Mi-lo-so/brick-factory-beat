namespace BrickFactoryBeat.Domain.Equipment;

public interface IEquipmentRepository
{
    Task AddAsync(Equipment equipment);
    Task DeleteAsync(string equipmentId);
    Task UpdateAsync(Equipment equipment);

    Task<Equipment?> GetByIdAsync(string id);
    Task<Equipment?> GetByNameAsync(string name);
    Task<List<Equipment>> GetAllAsync();
}