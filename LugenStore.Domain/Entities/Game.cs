using LugenStore.Domain.Common.Validation;
using LugenStore.Domain.Exceptions;

namespace LugenStore.Domain.Entities;

public class Game
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public List<Publisher> Publishers { get; private set; } = new List<Publisher>();
    public List<Genre> Genres { get; private set; } = new List<Genre>();

    protected Game() { }

    public Game(string name, decimal price, string description, List<Publisher> publishers, List<Genre> genres)
    {
        Validate(name, price);

        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        Description = description;
        Publishers = publishers;
        Genres = genres;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, decimal price, string description, List<Publisher> publishers, List<Genre> genres)
    {
        Validate(name, price);

        Name = name;
        Price = price;
        Description = description;
        Publishers = publishers;
        Genres = genres;
    }

    private static void Validate(string name, decimal price)
    {
        if (price < 0)
            throw new ValidationException("Price cannot be negative.");

        if (!ValidationPatterns.NameRegex.IsMatch(name))
        throw new ValidationException("Game name can only contain letters, numbers, spaces and basic punctuations");
    }
}
