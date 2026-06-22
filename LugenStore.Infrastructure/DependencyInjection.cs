using LugenStore.Application.Interfaces;
using LugenStore.Application.Services.Auth;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using LugenStore.Infrastructure.Repositories;
using LugenStore.Infrastructure.Security.Hash;
using LugenStore.Infrastructure.Security.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LugenStore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}