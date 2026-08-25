using LugenStore.Application.DTOs.User;
using LugenStore.Application.Interfaces;
using LugenStore.Domain.Exceptions;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace LugenStore.Application.Services;

public partial class UserService(IUserRepository _repository, IUnitOfWork _unitOfWork) : IUserService
{
    private static void SanitizeInput(UserBaseDto dto)
    {
        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name.Trim(), " ");
        dto.Email = GeneratedRegexes.WhitespaceRegex().Replace(dto.Email.Trim(), " ");
    }

    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var user = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"User with id {id} not found");

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<bool> UpdateAsync(UpdateUserDto dto)
    {
        SanitizeInput(dto);

        var existing = await _repository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException($"User with id {dto.Id} not found");

        var duplicate = await _repository.ExistsByEmailAsync(dto.Email);

        if (duplicate)
            throw new InvalidOperationException($"User with email {dto.Email} already exists");

        existing.Update(dto.Name, dto.Email);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var deleted = await _repository.DeleteAsync(id);

        if(!deleted)
            throw new NotFoundException($"User with id {id} not found.");
        
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();
    }
}
