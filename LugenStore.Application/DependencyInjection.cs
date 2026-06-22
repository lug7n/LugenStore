using LugenStore.Application.Interfaces;
using LugenStore.Application.Services;
using LugenStore.Application.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace LugenStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}