using LugenStore.API.Models;

namespace LugenStore.API.Repositories.Interfaces;

public interface IPublisherRepository
{
    Task<IEnumerable<Publisher>> GetAllAsync();
    Task<Publisher?> GetByIdAsync(Guid id);
    Task CreateAsync(Publisher publisher);
    Task UpdateAsync(Publisher publisher);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByNameExceptIdAsync(string name, Guid excludeId);
}
