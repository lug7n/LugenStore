using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Genre;

public class UpdateGenreDto : GenreBaseDto
{
    [Required]
    public Guid Id { get; set; }
}
