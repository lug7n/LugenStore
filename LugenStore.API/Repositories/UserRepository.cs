using LugenStore.API.Data;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.API.Repositories;

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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        var existingUser = await _context.User.FindAsync(user.Id);

        if (existingUser is null)
            return;

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;

        await _context.SaveChangesAsync();
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
