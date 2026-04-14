using LugenStore.API.DTOs.User;

namespace LugenStore.API.Services.Interfaces;

public interface IUserService 
{
    Task <UserResponseDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(UpdateUserDto dto);
    Task<bool> DeleteAsync(Guid id);
}
