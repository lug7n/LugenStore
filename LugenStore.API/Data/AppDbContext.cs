using LugenStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LugenStore.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Publisher> Publisher { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<CartItem> CartItems{ get; set; }
}
