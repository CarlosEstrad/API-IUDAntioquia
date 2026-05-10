using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicPlaylist.Api.Models.Songs;
using MusicPlaylist.Api.Models.PlayLists;

namespace MusicPlaylist.Api.Data.Configurations;

public class SongConfiguration : IEntityTypeConfiguration<SongsModels>
{
    public void Configure(EntityTypeBuilder<SongsModels> builder)
    {
        builder.ToTable("Songs");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Title).IsRequired().HasMaxLength(150);
        builder.Property(s => s.Artist).IsRequired().HasMaxLength(150);

        // CONFIGURACIÓN SIMPLIFICADA PARA RELACIÓN N:M
        builder.HasMany(s => s.Playlists)
               .WithMany(p => p.Songs)
               .UsingEntity<Dictionary<string, object>>(
                   "PlaylistSongs", // Nombre de la tabla en la BD
                   j => j.HasOne<PlayListModels>().WithMany().HasForeignKey("PlaylistId"),
                   j => j.HasOne<SongsModels>().WithMany().HasForeignKey("SongId")
               );
    }
}