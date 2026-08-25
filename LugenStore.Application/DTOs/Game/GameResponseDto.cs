namespace LugenStore.Application.DTOs.Game;

public class GameResponseDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<string> Publishers { get; set; } = new List<string>();
    public List<string> Genres { get; set; } = new List<string>();
}
