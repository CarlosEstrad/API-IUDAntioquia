using MusicPlaylist.Api.Models.Songs;
using MusicPlaylist.Api.Models.Users;

namespace MusicPlaylist.Api.Models.PlayLists
{
    public class PlayListModels
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relaciones
        public UserModels? User { get; set; }
        public ICollection<SongsModels> Songs { get; set; } = new List<SongsModels>();
    }
}
