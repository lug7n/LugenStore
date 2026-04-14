using LugenStore.API.DTOs.Auth;

namespace LugenStore.API.Services.Auth;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
