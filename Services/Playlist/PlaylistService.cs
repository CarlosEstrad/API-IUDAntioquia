using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Api.Data;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.PlayList;
using MusicPlaylist.Api.DTOs.Song;
using MusicPlaylist.Api.Interfaz.Playlist;
using MusicPlaylist.Api.Models.PlayLists;
using MusicPlaylist.Api.Models.Songs;
using MusicPlaylist.Api.Models.Users;

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
             .SqlQuery<PlaylistDto>($"SELECT * FROM vw_GetPlaylistsSongs WHERE UserId = {userId}")
             .ToListAsync();

            // 2.Transformamos y agrupamos usando LINQ
                var groupedPlaylists = playlists
                    .GroupBy(p => p.Id) // Agrupamos por el ID de la Playlist
                    .Select(group => new PlayListModels
                    {
                        Id = group.Key,
                        Name = group.First().Name,
                        Description = group.First().Description,
                        UserId = group.First().UserId,
                        CreatedAt = group.First().CreatedAt,
                        // Mapeamos los datos del usuario (opcional si lo necesitas)
                        User = new UserModels
                        {
                            Username = group.First().Username,
                            Email = group.First().Email
                        },
                        // Aquí recorremos todas las canciones que pertenecen a este "padre"
                        Songs = group
                            .Where(x => x.IdSong.HasValue && x.IdSong.Value > 0)// Validamos que exista una canción
                            .Select(s => new SongsModels
                            {
                                Id = s.IdSong!.Value,
                                Title = s.Title,
                                Artist = s.Artist,
                                Album = s.Album,
                                Duration = s.Duration
                            }).ToList()
                    })
                    .ToList();

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = groupedPlaylists.Any() ? "Lista de las playlist" : "El usuario no tiene Playlist asociadas",
                Data = groupedPlaylists
            };
        }
        #endregion

        #region Obtener todas las Playlist del sistema
        public async Task<LoginReplyModel> GetAllUserPlaylists()
        {
            // Consultamos directamente la Vista
            var playlists = await _context.Database
             .SqlQuery<PlaylistDto>($"SELECT * FROM vw_GetPlaylistsSongs")
             .ToListAsync();

            // 2.Transformamos y agrupamos usando LINQ
            var groupedPlaylists = playlists
                .GroupBy(p => p.Id) // Agrupamos por el ID de la Playlist
                .Select(group => new PlayListModels
                {
                    Id = group.Key,
                    Name = group.First().Name,
                    Description = group.First().Description,
                    UserId = group.First().UserId,
                    CreatedAt = group.First().CreatedAt,
                    // Mapeamos los datos del usuario (opcional si lo necesitas)
                    User = new UserModels
                    {
                        Username = group.First().Username,
                        Email = group.First().Email
                    },
                    // Aquí recorremos todas las canciones que pertenecen a este "padre"
                    Songs = group
                        .Where(x => x.IdSong.HasValue && x.IdSong.Value > 0)// Validamos que exista una canción
                        .Select(s => new SongsModels
                        {
                            Id = s.IdSong!.Value,
                            Title = s.Title,
                            Artist = s.Artist,
                            Album = s.Album,
                            Duration = s.Duration
                        }).ToList()
                })
                .ToList();

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = groupedPlaylists.Any() ? "Lista de las playlist" : "El usuario no tiene Playlist asociadas",
                Data = groupedPlaylists
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
        public async Task<LoginReplyModel> UpdatePlaylist(PlaylistUpdate playlistDto)
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
                Message = result != 0 ? "Playlist eliminada" : "No existe Playlist para ser eliminada",
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
                Flag = result > 0 ? true : false,
                Message = result > 0 ? "Canción añadida a la playlist" : "Está canción ya se encuentra asociada para esta PlayList, Intenta asociar otra canción.",
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
                Message = result != 0 ? "Canción removida de la playlist" : "No existe relación para ser eliminada de la PlayList",
                Data = result
            };
        }
        #endregion
    }
}