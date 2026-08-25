using LugenStore.Domain.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace LugenStore.Domain.Entities;

public class Publisher
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public List<Game> Games { get; private set; } = new List<Game>();


    public Publisher(string name)
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
            throw new ValidationException("Publisher name can only contain letters, numbers, spaces and basic punctuations");
    }
}