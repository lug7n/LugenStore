using LugenStore.Application.DTOs.Publisher;
using LugenStore.Application.Interfaces;
using LugenStore.Domain.Entities;
using LugenStore.Domain.Exceptions;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace LugenStore.Application.Services;

public partial class PublisherService(IPublisherRepository _repository, IUnitOfWork _unitOfWork) : IPublisherService
{
    private static void SanitizeInput (PublisherBaseDto dto)
    {
        dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name.Trim(), " ");
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
        SanitizeInput(dto);

        if (await _repository.ExistsByNameAsync(dto.Name))
            throw new ValidationException($"Publisher with name {dto.Name} already exists");

        var publisher = new Publisher(dto.Name);

        await _repository.CreateAsync(publisher);
        await _unitOfWork.SaveChangesAsync();

        return new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name
        };
    }

    public async Task<bool> UpdateAsync(UpdatePublisherDto dto)
    {
        SanitizeInput(dto);

        var existing = await _repository.GetByIdAsync(dto.Id)
            ?? throw new NotFoundException($"Publisher with id {dto.Id} not found");

        if (await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id))
            throw new InvalidOperationException($"Publisher with name {dto.Name} already exists");

        existing.Update(dto.Name);

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ValidationException("Id cannot be empty");

        var deleted = await _repository.DeleteAsync(id);

        if(!deleted)
            throw new NotFoundException($"Publisher with id {id} not found");

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();
    }
}
