using MusicPlaylist.Api.Models.PlayLists;

namespace MusicPlaylist.Api.Models.Songs
{
    public class SongsModels
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string? Album { get; set; }
        public int Duration { get; set; } // En segundos

        // Propiedad de navegación para la relación N:M
        public ICollection<PlayListModels> Playlists { get; set; } = new List<PlayListModels>();
    }
}
