using LugenStore.Domain.Entities;

namespace LugenStore.Infrastructure.Security.Token;

public interface ITokenService
{
    public string GenerateToken(User user);
}
