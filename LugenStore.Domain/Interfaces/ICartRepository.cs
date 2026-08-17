using LugenStore.Domain.Entities;

namespace LugenStore.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetActiveCartByUserIdAsync(Guid userId);
    Task CreateAsync(Cart cart);
    Task UpdateAsync(Cart cart);
}
