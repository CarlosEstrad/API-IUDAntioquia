using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.PlayList;
using MusicPlaylist.Api.DTOs.Song;
using MusicPlaylist.Api.Interfaz.Playlist;

namespace MusicPlaylist.Api.Controllers.Playlist
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        protected Reply oReply = new Reply();

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
        }

        #region Obtiene todas las playlists del usuario autenticado.
        /// GET: api/Playlist/GetUserPlaylists
        /// <summary>
        /// Obtiene todas las playlists del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Método que obtiene todas las playlists del usuario autenticado.
        /// </remarks>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpGet("GetUserPlaylists")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPlaylist()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _playlistService.GetUserPlaylists(userId);

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

        #region Obtiene todas las playlists del sistema.
        /// GET: api/Playlist/GetAllUserPlaylists
        /// <summary>
        /// Obtiene todas las playlists del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Método que obtiene todas las playlists del usuario autenticado.
        /// </remarks>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpGet("GetAllUserPlaylists")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUserPlaylists()
        {
            var result = await _playlistService.GetAllUserPlaylists();

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

        #region Crea una nueva playlist para el usuario logueado
        /// POST: api/Playlist/CreatePlaylist
        /// <summary>
        /// Crea una nueva playlist para el usuario logueado.
        /// </summary>
        /// <remarks>
        /// Método que crea una nueva playlist para el usuario logueado.
        /// </remarks>
        /// <param name="dto">Datos de la playlist.</param>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpPost("CreatePlaylist")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistRegistre dto)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await _playlistService.CreatePlaylist(userId, dto);

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

        #region Actualiza una playlist existente.
        /// PUT: api/Playlist/UpdatePlaylist
        /// <summary>
        /// Actualiza una playlist existente.
        /// </summary>
        /// <remarks>
        /// Método que actualiza una playlist existente.
        /// </remarks>
        /// <param name="dto">Datos de la playlist.</param>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpPut("UpdatePlaylist")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlaylist([FromBody] PlaylistUpdate dto)
        {
            var result = await _playlistService.UpdatePlaylist(dto);

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

        #region Elimina una playlist.
        /// DELETE: api/Playlist/DeletePlaylist
        /// <summary>
        /// Elimina una playlist.
        /// </summary>
        /// <remarks>
        /// Método que elimina una playlist..
        /// </remarks>
        /// <param name="id">identificador de la playlist.</param>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpDelete("DeletePlaylist/{id}")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeletePlaylist([FromRoute] int id)
        {
            var result = await _playlistService.DeletePlaylist(id);

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

        #region Asocia una canción a una playlist específica.
        /// POST: api/Playlist/AddSongToPlaylist
        /// <summary>
        /// Asocia una canción a una playlist específica.
        /// </summary>
        /// <remarks>
        /// Método que asocia una canción a una playlist específica.
        /// </remarks>
        /// <param name="dto">data de la canción para asociarla a la playlist.</param>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpPost("AddSongToPlaylist")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] AddSongDto dto)
        {
            var result = await _playlistService.AddSongToPlaylist(dto);

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

        #region Remueve una canción de una playlist.
        /// DELETE: api/Playlist/RemoveSongFromPlaylist/{playlistId}/{songId}
        /// <summary>
        /// Remueve una canción de una playlist.
        /// </summary>
        /// <remarks>
        /// Método que remueve una canción de una playlist.
        /// </remarks>
        /// <param name="playlistId">Identificador de la playlist la playlist.</param>
        /// <param name="songId">Identificador de la canción.</param>
        /// <returns>Respuesta estandarizada con el resultado.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpDelete("RemoveSongFromPlaylist/{playlistId}/{songId}")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSongFromPlaylist(int playlistId, int songId)
        {
            var result = await _playlistService.RemoveSongFromPlaylist(playlistId, songId);
            
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