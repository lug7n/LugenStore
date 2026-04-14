namespace LugenStore.API.Services.Security.Hash;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
