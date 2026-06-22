using LugenStore.Domain.Entities;

namespace LugenStore.Domain.Interfaces;

public interface IUserRepository
{
    //Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByCpfAsync(string cpf);
}
