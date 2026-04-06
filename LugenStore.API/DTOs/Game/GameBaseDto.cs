using System.ComponentModel.DataAnnotations;

namespace LugenStore.API.DTOs.Game;

public class GameBaseDto
{
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    [StringLength(2000, MinimumLength = 30)]
    public string Description { get; set; } = string.Empty;

    public Guid PublisherId { get; set; }

    public List<Guid> GenreId { get; set; } = new List<Guid>();
}