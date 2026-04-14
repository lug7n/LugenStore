using LugenStore.API.Models;

namespace LugenStore.API.Services.Security.Token;

public interface ITokenService
{
    public string GenerateToken(User user);
}
