using LugenStore.Domain.Entities;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.Infrastructure.Persistence.Repositories;

public class GameRepository(AppDbContext _context) : IGameRepository
{
    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _context.Games
            .Include(g => g.Publishers)
            .Include(g => g.Genres)
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _context.Games
            .Include(g => g.Publishers)
            .Include(g => g.Genres)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task CreateAsync(Game game)
    {
        await _context.Games.AddAsync(game);
    }

    public async Task UpdateAsync(Game game)
    {
        _context.Games.Update(game);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
       var rows = await _context.Games
           .Where(g => g.Id == id)
           .ExecuteDeleteAsync();

        return rows > 0;
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Games
            .AnyAsync(g => g.Name.ToLower() == name.ToLower());
    }
    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Games.
            AnyAsync(g => g.Id == id);
    }
    public async Task<bool> ExistsByNameExceptIdAsync(string name, Guid excludeId)
    {
        return await _context.Games
            .AnyAsync(g => g.Name.ToLower() == name.ToLower() && g.Id != excludeId);
    }
}
