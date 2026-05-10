using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Api.Models.PlayLists;
using MusicPlaylist.Api.Models.Songs;
using MusicPlaylist.Api.Models.Users;
using System.Reflection;

namespace MusicPlaylist.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<UserModels> Users { get; set; }
        public DbSet<PlayListModels> Playlists { get; set; }
        public DbSet<SongsModels> Songs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Esta línea mágica busca todas las clases que heredan de IEntityTypeConfiguration
            // en este mismo proyecto y las aplica automáticamente.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
