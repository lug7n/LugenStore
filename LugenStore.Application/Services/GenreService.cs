using LugenStore.Application.DTOs.Genre;
using LugenStore.Application.Interfaces;
using LugenStore.Domain.Common.Validation;
using LugenStore.Domain.Entities;
using LugenStore.Domain.Exceptions;
using LugenStore.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.Application.Services;

public partial class GenreService(IGenreRepository _repository) : IGenreService
{
    private static void SanitizeInput(GenreBaseDto dto)
    {
        dto.Name = dto.Name.Trim();
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
        SanitizeInput(dto);

        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException($"Genre with name {dto.Name} already exists");

        var genre = new Genre(dto.Name);

        await _repository.CreateAsync(genre);

        var createdGenre = await _repository.GetByIdAsync(genre.Id)
              ?? throw new NotFoundException($"Genre with id {genre.Id} not found after creation.");

        return new GenreResponseDto
        {
            Id = createdGenre.Id,
            Name = createdGenre.Name
        };
    }
    public async Task<bool> UpdateAsync(UpdateGenreDto dto)
    {
        SanitizeInput(dto);

        var duplicate = await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id);
        var genreExists = await _repository.GetByIdAsync(dto.Id);

        if (duplicate)
            throw new ValidationException($"Genre with name {dto.Name} already exists.");

        if(genreExists is null)
            throw new NotFoundException($"Genre with id {dto.Id} not found.");

        genreExists.Update(dto.Name);

        await _repository.UpdateAsync(genreExists);

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
