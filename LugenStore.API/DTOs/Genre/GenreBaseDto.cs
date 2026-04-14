using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Genre;

public class GenreBaseDto
{
    [Required(ErrorMessage = "Genre name is required")]
    [StringLength(30, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
