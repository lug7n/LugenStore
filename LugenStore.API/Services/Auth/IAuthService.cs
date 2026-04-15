using LugenStore.API.DTOs.Auth;
using LugenStore.API.DTOs.User;

namespace LugenStore.API.Services.Auth;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
