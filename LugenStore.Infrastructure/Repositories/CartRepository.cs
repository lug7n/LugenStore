using LugenStore.Domain.Entities;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.Infrastructure.Repositories;

public class CartRepository(AppDbContext _context) : ICartRepository
{
    public async Task<Cart?> GetActiveCartByUserIdAsync(Guid userId)
    {
        return await _context.Cart
            .Include(c => c.CartItens)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task CreateAsync(Cart cart)
    {
        await _context.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Cart.Update(cart);
        await _context.SaveChangesAsync();
    }

}
