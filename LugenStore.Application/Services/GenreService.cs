using LugenStore.Application.DTOs.Genre;
using LugenStore.Application.Interfaces;
using LugenStore.Domain.Entities;
using LugenStore.Domain.Exceptions;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace LugenStore.Application.Services;

public partial class GenreService(IGenreRepository _repository, IUnitOfWork _unitOfWork) : IGenreService
{
    private static void SanitizeInput(GenreBaseDto dto)
    {
        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name.Trim(), " ");
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
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty.");

        var genre = await _repository.GetByIdAsync(id);

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
        await _unitOfWork.SaveChangesAsync();

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
    public async Task<bool> UpdateAsync(UpdateGenreDto dto)
    {
        SanitizeInput(dto);

        var genreExists = await _repository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException($"Genre with id {dto.Id} not found.");

        if (await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id))
            throw new InvalidOperationException($"Genre with name {dto.Name} already exists");

        genreExists.Update(dto.Name);

        await _repository.UpdateAsync(genreExists);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var deleted = await _repository.DeleteAsync(id);

        if(!deleted)
            throw new NotFoundException($"Genre with id {id} not found."); 

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
     
    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();
    }
}
