using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.Song;
using MusicPlaylist.Api.Interfaz.Song;

namespace MusicPlaylist.Api.Controllers.Song
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : ControllerBase
    {
        private readonly ISongService _songService;
        protected Reply oReply = new Reply();

        public SongController(ISongService songService)
        {
            _songService = songService;
        }

        #region Obtiene todas las canciones del sistema.
        /// GET: api/Song/GetAllSongs
        /// <summary>
        /// Obtiene el catálogo completo de canciones disponibles.
        /// </summary>
        /// <remarks>
        /// Método que obtiene el catálogo completo de canciones disponibles.
        /// </remarks>
        /// <returns>Respuesta estandarizada con la lista de canciones.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpGet("GetAllSongs")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSongs()
        {
            var result = await _songService.GetAllSongs();
            return ProcessReply(result);
        }
        #endregion

        #region Registra una nueva canción en la base de datos.
        /// POST: api/Song/CreateSong
        /// <summary>
        /// Registra una nueva canción en la base de datos.
        /// </summary>
        /// <param name="dto">Datos de la canción (Title, Artist, Album, Duration).</param>
        /// <remarks>
        /// Método que registra una nueva canción en la base de datos.
        /// </remarks>
        /// <returns>Respuesta estandarizada con los datos creados para la canción.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpPost("CreateSong")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSong([FromBody] SongDto dto)
        {
            var result = await _songService.CreateSong(dto);
            return ProcessReply(result);
        }
        #endregion

        #region Actualiza la información de una canción mediante su ID.
        /// PUT: api/Song/UpdateSong
        /// <summary>
        /// Actualiza la información de una canción mediante su ID.
        /// </summary>
        /// <param name="dto">Datos de la canción (Title, Artist, Album, Duration).</param>
        /// <remarks>
        /// Método que actualiza la información de una canción mediante su ID.
        /// </remarks>
        /// <returns>Respuesta estandarizada con los datos actualizados de una canción.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpPut("UpdateSong")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSong([FromBody] SongDto dto)
        {
            var result = await _songService.UpdateSong(dto);
            return ProcessReply(result);
        }
        #endregion

        #region Elimina una canción del sistema y sus asociaciones en playlists.
        /// PUT: api/Song/UpdateSong
        /// <summary>
        /// Elimina una canción del sistema y sus asociaciones en playlists.
        /// </summary>
        /// <param name="id">Identificador de la canción.</param>
        /// <remarks>
        /// Método que elimina una canción del sistema y sus asociaciones en playlists.
        /// </remarks>
        /// <returns>Respuesta estandarizada con sastifactoria.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpDelete("DeleteSong/{id}")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var result = await _songService.DeleteSong(id);
            return ProcessReply(result);
        }
        #endregion

        #region Método Auxiliar de Respuesta
        private IActionResult ProcessReply(LoginReplyModel result)
        {
            oReply.Ok = result.Flag;
            oReply.Message = result.Message;
            oReply.Data = result.Data;

            return result.Status switch
            {
                200 => Ok(oReply),
                400 => BadRequest(oReply),
                404 => NotFound(oReply),
                _ => StatusCode(result.Status, oReply)
            };
        }
        #endregion
    }
}
