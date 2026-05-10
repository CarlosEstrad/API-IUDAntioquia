using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicPlaylist.Api.Models.Users;

namespace MusicPlaylist.Api.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserModels>
{
    public void Configure(EntityTypeBuilder<UserModels> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        // Aquí podrías agregar más reglas, como:
        // builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
    }
}