using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicPlaylist.Api.Models.PlayLists;
using MusicPlaylist.Api.Models.Users;

namespace MusicPlaylist.Api.Data.Configurations;

public class PlaylistConfiguration : IEntityTypeConfiguration<PlayListModels>
{
    public void Configure(EntityTypeBuilder<PlayListModels> builder)
    {
        builder.ToTable("Playlists");
        builder.HasKey(p => p.Id);

        // Configuración de la relación 1:N con Usuario
        builder.HasOne<UserModels>()
               .WithMany()
               .HasForeignKey(p => p.UserId);
    }
}