using LugenStore.Domain.Entities;

namespace LugenStore.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(Guid userId, bool IsActive);
    Task CreateAsync(Cart cart);
    Task UpdateAsync(Cart cart);
}
