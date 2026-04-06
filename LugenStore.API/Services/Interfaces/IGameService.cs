using LugenStore.API.DTOs.Game;

namespace LugenStore.API.Services.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameResponseDto>> GetAllAsync();
    Task<GameResponseDto?> GetByIdAsync(Guid id);
    Task<GameResponseDto> CreateAsync (CreateGameDto dto);
    Task<bool> UpdateAsync (UpdateGameDto dto);
    Task<bool> DeleteAsync(Guid id);
}
