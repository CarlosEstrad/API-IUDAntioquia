namespace MusicPlaylist.Api.DTOs.Song
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public int Duration { get; set; } // En segundos, como en tus INSERT
    }

    public class AddSongDto
    {
        public int PlaylistId { get; set; }
        public int SongId { get; set; }
    }
}
