using LugenStore.API.Models;

namespace LugenStore.API.Repositories.Interfaces;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(Guid id);
    Task CreateAsync(Game game);
    Task UpdateAsync(Game game);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> ExistsByNameExceptIdAsync(string name, Guid excludeId);
}