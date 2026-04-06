using LugenStore.API.DTOs.Publisher;

namespace LugenStore.API.Services.Interfaces;

public interface IPublisherService
{
    Task<IEnumerable<PublisherResponseDto>> GetAllAsync();
    Task<PublisherResponseDto?> GetByIdAsync(Guid id);
    Task<PublisherResponseDto> CreateAsync(CreatePublisherDto dto);
    Task<bool> UpdateAsync(UpdatePublisherDto dto);
    Task<bool> DeleteAsync (Guid id);
}
