using LugenStore.API.Models;

namespace LugenStore.API.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(Guid id);
    Task CreateAsync(Genre genre);
    Task UpdateAsync(Genre genre);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByIdAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<bool> ExistsByNameExceptIdAsync(string name, Guid excludeId);
}
