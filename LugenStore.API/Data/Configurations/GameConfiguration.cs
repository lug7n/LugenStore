using LugenStore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LugenStore.API.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(x => x.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Price)
            .IsRequired()
            .HasPrecision(10,2);

        builder.Property(g => g.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(g => g.CreatedAt)
            .IsRequired();

        builder.HasOne(g => g.Publisher)
            .WithMany(p => p.Games)
            .HasForeignKey(g => g.PublisherId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete to avoid deleting publisher when a game is deleted

        builder.HasMany(g => g.Genres)
            .WithMany(g => g.Games)
            .UsingEntity(j => j.ToTable("GameGenres")); // Configure many-to-many relationship with a join table
    }
}
