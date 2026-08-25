using System.ComponentModel.DataAnnotations;

namespace LugenStore.Application.DTOs.Game;

public class GameBaseDto
{
    [Required(ErrorMessage = "Game name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Game price is required")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(2000, MinimumLength = 30)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Publisher Id is required")]
    public List<Guid> PublisherId { get; set; } = new List<Guid>();

    [Required(ErrorMessage = "Genre Id is required")]
    public List<Guid> GenreId { get; set; } = new List<Guid>();
}
