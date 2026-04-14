using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Publisher;

public class PublisherBaseDto
{
    [Required(ErrorMessage = "Publisher name is required.")]
    [StringLength(30, MinimumLength = 2)]
    public string Name { get; set; } = null!;
}
