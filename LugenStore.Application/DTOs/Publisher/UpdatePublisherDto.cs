using System.ComponentModel.DataAnnotations;

namespace LugenStore.Application.DTOs.Publisher;

public class UpdatePublisherDto : PublisherBaseDto
{
    [Required]
    public Guid Id { get; set; }
}
