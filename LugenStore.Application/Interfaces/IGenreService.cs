using LugenStore.Application.DTOs.Genre;

namespace LugenStore.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreResponseDto>> GetAllAsync();
    Task<GenreResponseDto?> GetByIdAsync(Guid id);
    Task<GenreResponseDto> CreateAsync (CreateGenreDto dto);
    Task<bool> UpdateAsync (UpdateGenreDto dto);
    Task<bool> DeleteAsync(Guid id);
}
