using MusicPlaylist.Api.Interfaz.Playlist;
using MusicPlaylist.Api.Interfaz.Security;
using MusicPlaylist.Api.Interfaz.Song;
using MusicPlaylist.Api.Services.Playlist;
using MusicPlaylist.Api.Services.Security;
using MusicPlaylist.Api.Services.Song;

namespace MusicPlaylist.Api.Data.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // agrupar todos los servicios
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPlaylistService, PlaylistService>();
            services.AddScoped<ISongService, SongService>();

            return services;
        }
    }
}
