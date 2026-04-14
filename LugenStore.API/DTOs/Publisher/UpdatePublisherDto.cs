using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Publisher;

public class UpdatePublisherDto : PublisherBaseDto
{
    [Required]
    public Guid Id { get; set; }
}
