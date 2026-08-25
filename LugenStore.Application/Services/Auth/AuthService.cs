using LugenStore.Application.DTOs.Auth;
using LugenStore.Application.DTOs.User;
using LugenStore.Domain.Entities;
using LugenStore.Domain.Exceptions;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Security.Hash;
using LugenStore.Infrastructure.Security.Token;
using LugenStore.Application.Validators;
using System.Text.RegularExpressions;
using LugenStore.Infrastructure.Persistence;

namespace LugenStore.Application.Services.Auth;

public partial class AuthService(IUserRepository _repository, IUnitOfWork _unitOfWork, IPasswordHasher _passwordHasher, ITokenService _tokenService) : IAuthService
{
    private static void SanitizeInput(RegisterDto dto)
    {
        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name.Trim(), " ");
        dto.Email = dto.Email.Trim().ToLower();
        dto.Cpf = GeneratedRegexes.WhitespaceRegex().Replace(dto.Cpf.Trim(), " ");
    }

    private static void SanitizeInputLogin(LoginDto dto)
    {
        dto.Email = dto.Email.Trim().ToLower();
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
    {
        SanitizeInput(dto);

        if (dto.Password != dto.ConfirmPassword)
            throw new ValidationException("Passwords do not match");

        if (await _repository.ExistsByCpfAsync(dto.Cpf))
            throw new InvalidOperationException("CPF already registered");

        if (!CpfValidator.IsValid(dto.Cpf))
            throw new ValidationException("Invalid CPF");

        if (await _repository.ExistsByEmailAsync(dto.Email))
            throw new InvalidOperationException("Email already registered");

        if (GeneratedRegexes.WhitespaceCharRegex().IsMatch(dto.Password))
            throw new ValidationException("Password cannot contain spaces");

        var hash = _passwordHasher.HashPassword(dto.Password);

        var user = new User(dto.Name, dto.Cpf, dto.Email, hash);

        await _repository.CreateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        SanitizeInputLogin(dto);

        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials (Email)");

        var isValid = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);

        if (!isValid)
            throw new UnauthorizedAccessException("Invalid credentials (Password)");

        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }


    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();

        [GeneratedRegex(@"\s")]
        internal static partial Regex WhitespaceCharRegex();

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
        internal static partial Regex EmailRegex();
    }
}
