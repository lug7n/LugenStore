namespace LugenStore.Domain.Entities;

public class Publisher
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Game> Games { get; set; } = new List<Game>();
}
