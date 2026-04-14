using LugenStore.API.Common.Validation;
using LugenStore.API.DTOs.Publisher;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _repository;

    public PublisherService(IPublisherRepository repository)
    {
        _repository = repository;
    }

    private async Task ValidatePublisher (PublisherBaseDto dto)
    {
        dto.Name = dto.Name.Trim();
        dto.Name = Regex.Replace(dto.Name, @"\s+", " ");

        if (!ValidationPatterns.NameRegex.IsMatch(dto.Name))
            throw new ValidationException("Publisher name can only contain letters, numbers, spaces, and basic punctuation.");
    }
    public async Task<IEnumerable<PublisherResponseDto>> GetAllAsync()
    {
        var publisher = await _repository.GetAllAsync();

        return publisher.Select(publisher => new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name
        });
    }

    public async Task<PublisherResponseDto?> GetByIdAsync(Guid id)
    {
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
        var duplicate = await _repository.ExistsByNameAsync(dto.Name);

        await ValidatePublisher(dto);

        if (duplicate)
            throw new ValidationException($"Publisher {dto.Name} already exists in our database");

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
        var duplicate = await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id);
        var publisherExists = await _repository.ExistsByIdAsync(dto.Id);

        await ValidatePublisher(dto);

        if (duplicate)
            throw new ValidationException($"Publisher with name {dto.Name} already exists");

        if (!publisherExists)
            throw new NotFoundException($"Publisher with id {dto.Id} not found");

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
        var deleted = await _repository.DeleteAsync(id);

        if(!deleted)
            throw new NotFoundException($"Publisher with id {id} not found");

        return true;
    }
}
