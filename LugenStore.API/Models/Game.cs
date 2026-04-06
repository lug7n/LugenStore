namespace LugenStore.API.Models;

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; } = null!;

    public List<Genre> Genres { get; set; } = new List<Genre>();
}
