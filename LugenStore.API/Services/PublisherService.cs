using LugenStore.API.Common.Validation;
using LugenStore.API.DTOs.Publisher;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services;

public partial class PublisherService(IPublisherRepository _repository) : IPublisherService
{
    private static void ValidatePublisher (PublisherBaseDto dto)
    {
        dto.Name = dto.Name.Trim();
        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name, " ");

        if (!ValidationPatterns.NameRegex.IsMatch(dto.Name))
            throw new ValidationException("Publisher name can only contain letters, numbers, spaces, and basic punctuation.");
    }
    public async Task<IEnumerable<PublisherResponseDto>> GetAllAsync()
    {
        var publishers = await _repository.GetAllAsync();

        return publishers.Select(publisher => new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name
        });
    }

    public async Task<PublisherResponseDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var publisher = await _repository.GetByIdAsync(id);

        if (publisher is null)
            throw new NotFoundException($"Publisher with id {id} not found");

        return new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name   
        };
    }

    public async Task<PublisherResponseDto> CreateAsync(CreatePublisherDto dto)
    {
        ValidatePublisher(dto);

        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new InvalidOperationException($"Publisher with name {dto.Name} already exists");
        
        var publisher = new Publisher
        {
            Id = Guid.NewGuid(), 
            Name = dto.Name
        };

        await _repository.CreateAsync(publisher);

        return new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name
        };
    }

    public async Task<bool> UpdateAsync(UpdatePublisherDto dto)
    {
        ValidatePublisher(dto);

        if (await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id))
            throw new InvalidOperationException($"Publisher with name {dto.Name} already exists");

        var existing = await _repository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException($"Publisher with id {dto.Id} not found");

        var publisher = new Publisher
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
        };

        await _repository.UpdateAsync(publisher);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var deleted = await _repository.DeleteAsync(id);

        if(!deleted)
            throw new NotFoundException($"Publisher with id {id} not found");

        return true;
    }

    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();
    }
}
