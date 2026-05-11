namespace MusicPlaylist.Api.DTOs.PlayList
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }

    public class PlaylistRegistre
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class PlaylistUpdate : PlaylistRegistre
    {
        public int Id { get; set; }
    }
}
