using System.ComponentModel.DataAnnotations;

namespace LugenStore.Application.DTOs.Genre;

public class UpdateGenreDto : GenreBaseDto
{
    [Required]
    public Guid Id { get; set; }
}
