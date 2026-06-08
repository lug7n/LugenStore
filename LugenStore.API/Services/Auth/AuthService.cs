using LugenStore.API.DTOs.Auth;
using LugenStore.API.DTOs.User;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Security.Hash;
using LugenStore.API.Services.Security.Token;
using LugenStore.API.Validators;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services.Auth;

public class AuthService(IUserRepository _repository, IPasswordHasher _passwordHasher, ITokenService _tokenService) : IAuthService
{
    private static void NormalizeRegister(RegisterDto dto)
    {
        dto.Name = dto.Name.Trim();
        dto.Email = dto.Email.Trim().ToLower();
        dto.Cpf = dto.Cpf.Trim();

        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name, " ");
        dto.Cpf = GeneratedRegexes.WhitespaceRegex().Replace(dto.Cpf, " ");

        if (dto.Email.Contains(' '))
            throw new ValidationException("Email cannot contain spaces");
    }

    private static void NormalizeLogin(LoginDto dto)
    {
        dto.Email = dto.Email.Trim().ToLower();

        if (dto.Email.Contains(' '))
            throw new ValidationException("Email cannot contain spaces");
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterDto dto)
    {
        NormalizeRegister(dto);

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

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Cpf = dto.Cpf,
            PasswordHash = hash
        };

        await _repository.CreateAsync(user);

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
        NormalizeLogin(dto);

        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials");

        var isValid = _passwordHasher.VerifyPassword(dto.Password, user.PasswordHash);

        if (!isValid)
            throw new UnauthorizedAccessException("Invalid credentials");

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
}

internal static partial class GeneratedRegexes
{
    [GeneratedRegex(@"\s+")]
    internal static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"\s")]
    internal static partial Regex WhitespaceCharRegex();
}
