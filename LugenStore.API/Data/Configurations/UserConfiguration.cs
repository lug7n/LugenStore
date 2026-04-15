using LugenStore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LugenStore.API.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .IsFixedLength();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);


        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Cpf).IsUnique();
    }
}
