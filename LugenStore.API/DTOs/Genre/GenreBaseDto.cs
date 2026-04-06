using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Genre;

public class GenreBaseDto
{
    [StringLength(30, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
