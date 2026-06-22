using LugenStore.Application.DTOs.Auth;
using LugenStore.Application.DTOs.User;

namespace LugenStore.Application.Services.Auth;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
