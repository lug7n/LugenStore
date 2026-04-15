using LugenStore.API.Data;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.API.Repositories
{
    public class GenreRepository(AppDbContext _context) : IGenreRepository
    {
        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre?> GetByIdAsync(Guid id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task CreateAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Genre genre)
        {
            var existingGenre = _context.Genres.FindAsync(genre.Id);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var rows = await _context.Genres
                .Where(g => g.Id == id)
                .ExecuteDeleteAsync();

            return rows > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _context.Genres
                .AnyAsync(g => g.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Genres
                .AnyAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> ExistsByNameExceptIdAsync(string name, Guid excludeId)
        {
            return await _context.Games
                .AnyAsync(g => g.Name.ToLower() == name.ToLower() && g.Id != excludeId);
        }
    }
}
