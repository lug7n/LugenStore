using LugenStore.API.DTOs.User;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services;

public partial class UserService(IUserRepository _repository) : IUserService
{

    private static void Normalize(UserBaseDto dto)
    {
        dto.Name = dto.Name.Trim();
        dto.Email = dto.Email.Trim();

        dto.Name = WhitespaceRegex().Replace(dto.Name, " ");
        dto.Email = WhitespaceRegex().Replace(dto.Email, " ");
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var user = await _repository.GetByIdAsync(id);

        if (user is null)
            throw new ArgumentNullException(nameof(id));

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<bool> UpdateAsync(UpdateUserDto dto)
    {
        Normalize(dto);

        var duplicate = await _repository.ExistsByEmailAsync(dto.Email);

        if (duplicate)
            throw new InvalidOperationException($"User with email {dto.Email} already exists");

        var userExists = await _repository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException($"User with id {dto.Id} not found");

        var user = new User
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            CreatedAt = userExists.CreatedAt
        };

        await _repository.UpdateAsync(user);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var deleted = await _repository.DeleteAsync(id);

        if (!deleted)
            throw new NotFoundException($"User with id {id} not found.");

        return true;
    }

    [GeneratedRegex(@"\s+")]
    internal static partial Regex WhitespaceRegex();
}