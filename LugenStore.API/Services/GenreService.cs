using LugenStore.API.Common.Validation;
using LugenStore.API.DTOs.Genre;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services;

public partial class GenreService(IGenreRepository _repository) : IGenreService
{
    private static void ValidateGenre(GenreBaseDto dto)
    {
        dto.Name = dto.Name.Trim();
        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name, " ");

        if (!ValidationPatterns.NameRegex.IsMatch(dto.Name))
            throw new ValidationException("Genre name can only contain letters, numbers, spaces, and basic punctuation.");
    }

    public async Task<IEnumerable<GenreResponseDto>> GetAllAsync()
    {
        var genre = await _repository.GetAllAsync();

        return genre.Select(genre => new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name,
        });
    }

    public async Task<GenreResponseDto?> GetByIdAsync(Guid id)
    {
        var genre = await _repository.GetByIdAsync(id);

        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty.");

        if (genre is null)
            throw new NotFoundException($"Genre with id {id} not found.");

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public async Task<GenreResponseDto> CreateAsync(CreateGenreDto dto)
    {
        ValidateGenre(dto);

        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException($"Genre with name {dto.Name} already exists");
        
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        await _repository.CreateAsync(genre);

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
    public async Task<bool> UpdateAsync(UpdateGenreDto dto)
    {
        ValidateGenre(dto);

        var duplicate = await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id);
        var genreExists = await _repository.ExistsByIdAsync(dto.Id);

        if (duplicate)
            throw new ValidationException($"Genre with name {dto.Name} already exists.");

        if(!genreExists)
            throw new NotFoundException($"Genre with id {dto.Id} not found.");

        var genre = new Genre
        {
            Id = dto.Id,
            Name = dto.Name
        };

        await _repository.UpdateAsync(genre);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var deleted = await _repository.DeleteAsync(id);

        if(!deleted)
            throw new NotFoundException($"Genre with id {id} not found."); 

        return true;
    }

    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();
    }
}
