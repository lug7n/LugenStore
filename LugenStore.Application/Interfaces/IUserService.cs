using LugenStore.Application.DTOs.User;

namespace LugenStore.Application.Interfaces;

public interface IUserService 
{
    Task <UserResponseDto?> GetByIdAsync(Guid id);
    Task<bool> UpdateAsync(UpdateUserDto dto);
    Task<bool> DeleteAsync(Guid id);
}
