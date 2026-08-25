using LugenStore.Domain.Entities;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext _context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.User.FindAsync(id);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task CreateAsync(User user)
    {
        await _context.User.AddAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        _context.User.Update(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rows = await _context.User
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        return rows > 0;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.User
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> ExistsByCpfAsync(string cpf)
    {
        return await _context.User
            .AnyAsync(u => u.Cpf == cpf);
    }
}
