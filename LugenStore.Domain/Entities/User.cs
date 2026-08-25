using LugenStore.Domain.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace LugenStore.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; private set; }


    public User(string name, string cpf, string email, string passwordHash) 
    {
        Validate(name, cpf, email, passwordHash);

        Id = Guid.NewGuid();
        Name = name;
        Cpf = cpf;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public static void Validate(string name, string cpf, string email, string passwordHash)
    {
        if (!ValidationPatterns.NameRegex.IsMatch(name))
            throw new ValidationException("User name can only contain letters, spaces and basic punctuations");

      
    }
}
