using LugenStore.Domain.Common.Validation;
using LugenStore.Domain.Exceptions;

namespace LugenStore.Domain.Entities;

public class Genre
{

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public List<Game> Games { get; private set; } = new List<Game>();

    public Genre(string name)
    {
        Validate(name);

        Id = Guid.NewGuid();
        Name = name;
    }

    public void Update(string name)
    {
        Validate(name);

        Name = name;
    }

    public static void Validate(string name)
    {
        if (!ValidationPatterns.NameRegex.IsMatch(name))
            throw new ValidationException("Game name can only contain letters, numbers, spaces and basic punctuations");
    }
}
