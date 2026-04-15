using LugenStore.API.Data;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.API.Repositories;

public class PublisherRepository(AppDbContext _context) : IPublisherRepository
{
    public async Task<IEnumerable<Publisher>> GetAllAsync()
    {
        return await _context.Publisher.ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(Guid id)
    {
        return await _context.Publisher.FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task CreateAsync(Publisher publisher)
    {
        await _context.Publisher.AddAsync(publisher);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Publisher publisher)
    {
        var existingPublisher = await _context.Publisher.FindAsync(publisher.Id);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rows = await _context.Publisher
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        return rows > 0; // Returns true if a publisher was deleted, false otherwise
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Publisher.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Publisher.AnyAsync(p => p.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> ExistsByNameExceptIdAsync(string name, Guid excludeId)
    {
        return await _context.Publisher.AnyAsync(p => p.Name == name && p.Id != excludeId);
    }
}
