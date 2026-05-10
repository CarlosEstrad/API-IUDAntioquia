using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.Song;

namespace MusicPlaylist.Api.Interfaz.Song
{
    public interface ISongService
    {
        Task<LoginReplyModel> GetAllSongs();
        Task<LoginReplyModel> CreateSong(SongDto songDto);
        Task<LoginReplyModel> UpdateSong(SongDto songDto);
        Task<LoginReplyModel> DeleteSong(int id);
    }
}
