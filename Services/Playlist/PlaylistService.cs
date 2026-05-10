using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Api.Data;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.PlayList;
using MusicPlaylist.Api.DTOs.Song;
using MusicPlaylist.Api.Interfaz.Playlist;

namespace MusicPlaylist.Api.Services.Playlist
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ApplicationDbContext _context;

        public PlaylistService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Obtiene las Playlist por usuario
        public async Task<LoginReplyModel> GetUserPlaylists(int userId)
        {
            // Consultamos directamente la Vista
            var playlists = await _context.Database
             .SqlQuery<PlaylistDto>($"SELECT * FROM vw_GetPlaylists WHERE UserId = {userId}")
             .ToListAsync();

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = playlists.Any() ? "Lista de las playlist" : "El usuario no tiene Playlist asociadas",
                Data = playlists
            };
        }
        #endregion

        #region Obtener todas las Playlist del sistema
        public async Task<LoginReplyModel> GetAllUserPlaylists()
        {
            // Consultamos directamente la Vista
            var playlists = await _context.Database
             .SqlQuery<PlaylistDto>($"SELECT * FROM vw_GetPlaylists")
             .ToListAsync();

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = playlists.Any() ? "Lista de las playlist" : "No existen playlist",
                Data = playlists
            };
        }
        #endregion

        #region Crea una nueva playlist para el usuario logueado
        public async Task<LoginReplyModel> CreatePlaylist(int userId, PlaylistRegistre playlistDto)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CreatePlaylist @UserId = {0}, @Name = {1}, @Description = {2}",
                userId, playlistDto.Name, playlistDto.Description);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = result != null ? "Playlist creada" : "Error al crear",
                Data = result
            };
        }
        #endregion

        #region Actualiza una playlist existente.
        public async Task<LoginReplyModel> UpdatePlaylist(PlaylistDto playlistDto)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdatePlaylist @Id = {0}, @Name = {1}, @Description = {2}",
                playlistDto.Id, playlistDto.Name, playlistDto.Description);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = result != null ? "Playlist actualizada" : "No se pudo actualizar",
                Data = result
            };
        }
        #endregion

        #region Elimina una playlist.
        public async Task<LoginReplyModel> DeletePlaylist(int id)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_DeletePlaylist @Id = {0}", id);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = result != null ? "Playlist eliminada" : "No se pudo eliminar",
                Data = result
            };
        }
        #endregion

        #region Asocia una canción a una playlist específica.
        public async Task<LoginReplyModel> AddSongToPlaylist(AddSongDto dto)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_AddSongToPlaylist @PlaylistId = {0}, @SongId = {1}",
                dto.PlaylistId, dto.SongId);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = result != null ? "Canción añadida a la playlist" : "No se pudo añadir la canción",
                Data = result
            };

        }
        #endregion

        #region Remueve una canción de una playlist.
        public async Task<LoginReplyModel> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM PlaylistSongs WHERE PlaylistId = {0} AND SongId = {1}",
                playlistId, songId);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = result != null ? "Canción removida de la playlist" : "La canción no estaba en la lista",
                Data = result
            };
        }
        #endregion
    }
}