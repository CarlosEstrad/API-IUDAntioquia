using Microsoft.EntityFrameworkCore;
using MusicPlaylist.Api.Data;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.Song;
using MusicPlaylist.Api.Interfaz.Song;

namespace MusicPlaylist.Api.Services.Song
{
    public class SongService : ISongService
    {
        private readonly ApplicationDbContext _context;

        public SongService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Obtiene el catálogo completo de canciones disponibles.
        public async Task<LoginReplyModel> GetAllSongs()
        {
            try
            {
                // Consultamos la vista definida en SQL
                var songs = await _context.Songs
                    .FromSqlRaw("SELECT * FROM vw_GetAllSongs")
                    .ToListAsync();

                if (songs == null || !songs.Any())
                {
                    return new LoginReplyModel
                    {
                        Status = 200,
                        Flag = true,
                        Message = "No se encontraron canciones en el catálogo.",
                        Data = new List<SongDto>()
                    };
                }

                return new LoginReplyModel
                {
                    Status = 200,
                    Flag = true,
                    Message = "Catálogo de canciones recuperado con éxito.",
                    Data = songs
                };
            }
            catch (Exception ex)
            {
                // Manejo de errores logueando la excepción si es necesario
                return new LoginReplyModel
                {
                    Status = 500,
                    Flag = false,
                    Message = "Error interno al obtener las canciones: " + ex.Message
                };
            }
        }
        #endregion

        #region Registra una nueva canción en la base de datos.
        public async Task<LoginReplyModel> CreateSong(SongDto songDto)
        {
            // Usamos los parámetros en el orden del SP
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CreateSong @Title={0}, @Artist={1}, @Album={2}, @Duration={3}",
                songDto.Title, songDto.Artist, songDto.Album, songDto.Duration);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = "Canción registrada exitosamente",
                Data = result
            };
        }
        #endregion

        #region Actualiza la información de una canción mediante su ID.
        public async Task<LoginReplyModel> UpdateSong(SongDto songDto)
        {
            var result = await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateSong @Id={0}, @Title={1}, @Artist={2}, @Album={3}, @Duration={4}",
                songDto.Id, songDto.Title, songDto.Artist, songDto.Album, songDto.Duration);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = "Datos de la canción actualizados",
                Data = result
            };
        }
        #endregion

        #region Elimina una canción del sistema y sus asociaciones en playlists.
        public async Task<LoginReplyModel> DeleteSong(int id)
        {
            // IMPORTANTE: Primero limpiamos la relación en la tabla intermedia 
            // para evitar errores de llave foránea.
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM PlaylistSongs WHERE SongId = {0}", id);

            // Ahora sí eliminamos la canción
            var result = await _context.Database.ExecuteSqlRawAsync("DELETE FROM Songs WHERE Id = {0}", id);

            return new LoginReplyModel
            {
                Status = 200,
                Flag = true,
                Message = "Canción eliminada del sistema",
                Data = result
            };
        }
        #endregion
    }
}
