using LugenStore.Domain.Entities;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.Infrastructure.Persistence.Repositories;

public class CartRepository(AppDbContext _context) : ICartRepository
{
    public async Task<Cart?> GetCartByUserIdAsync(Guid userId, bool isActive)
    {
        return await _context.Cart
            .Include(c => c.CartItens)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task CreateAsync(Cart cart)
    {
        await _context.AddAsync(cart);
    }

    public async Task UpdateAsync(Cart cart)
    {
        _context.Cart.Update(cart);
    }

}
