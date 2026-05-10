using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.PlayList;
using MusicPlaylist.Api.DTOs.Song;

namespace MusicPlaylist.Api.Interfaz.Playlist
{
    public interface IPlaylistService
    {
        Task<LoginReplyModel> GetUserPlaylists(int userId);
        Task<LoginReplyModel> GetAllUserPlaylists();
        Task<LoginReplyModel> CreatePlaylist(int userId, PlaylistRegistre playlistDto);
        Task<LoginReplyModel> UpdatePlaylist(PlaylistDto playlistDto);
        Task<LoginReplyModel> DeletePlaylist(int id);
        Task<LoginReplyModel> AddSongToPlaylist(AddSongDto dto);
        Task<LoginReplyModel> RemoveSongFromPlaylist(int playlistId, int songId);
    }
}
