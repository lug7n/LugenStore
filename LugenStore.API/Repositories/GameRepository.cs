using LugenStore.API.Data;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.API.Repositories;

public class GameRepository : IGameRepository
{

    private readonly AppDbContext _context;

    public GameRepository(AppDbContext context) 
    {
        _context = context;
    }

    public async Task<IEnumerable<Game>> GetAllAsync()
    {
        return await _context.Games
            .Include(g => g.Publisher)
            .Include(g => g.Genres)
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(Guid id)
    {
        return await _context.Games
            .Include(g => g.Publisher)
            .Include(g => g.Genres)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task CreateAsync(Game game)
    {
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Game game)
    {
        var existingGame = await _context.Games.FindAsync(game.Id);

        if (existingGame is null)
            return;

        existingGame.Name = game.Name;
        existingGame.Price = game.Price;
        existingGame.Publisher = game.Publisher;    
        existingGame.Genres = game.Genres;
        existingGame.Description = game.Description;

        await _context.SaveChangesAsync();
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
