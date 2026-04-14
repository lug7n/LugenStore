using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Game;

public class GameBaseDto
{
    [Required(ErrorMessage = "Game name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Game price is required")]
    public decimal Price { get; set; }

    [StringLength(2000, MinimumLength = 30)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Publisher id is required")]
    public Guid PublisherId { get; set; }

    [Required(ErrorMessage = "Genre id is required")]
    public List<Guid> GenreId { get; set; } = new List<Guid>();
}