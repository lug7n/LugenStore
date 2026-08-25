using LugenStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LugenStore.Infrastructure.Persistence.Configurations;

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

        builder.HasMany(g => g.Publishers)
            .WithMany(p => p.Games)
            .UsingEntity<Dictionary<string, object>>(
                "GamePublishers",
                j => j.HasOne<Publisher>()
                      .WithMany()
                      .HasForeignKey("PublisherId")
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Game>()
                      .WithMany()
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Cascade));

        builder.HasMany(g => g.Genres)
            .WithMany(g => g.Games)
            .UsingEntity<Dictionary<string, object>>(
                "GameGenres",
                j => j.HasOne<Genre>()
                      .WithMany()
                      .HasForeignKey("GenreId")
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Game>()
                      .WithMany()
                      .HasForeignKey("GameId")
                      .OnDelete(DeleteBehavior.Cascade));
            
        builder.HasIndex(g => g.Name);
    }
}
