using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.Login;
using MusicPlaylist.Api.Interfaz.Security;
using MusicPlaylist.Api.Models.Users;

namespace MusicPlaylist.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public Reply oReply = new Reply();

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Realiza la autenticación de un usuario para obtener un token de acceso.
        /// GET: api/ListarUsuarios
        /// <summary>
        /// Realiza la autenticación de un usuario para obtener un token de acceso.
        /// </summary>
        /// <remarks>
        /// Método que realiza la autenticación de un usuario para obtener un token de acceso.
        /// </remarks>
        /// <param name="loginDto">Objeto que contiene las credenciales del usuario (Email y Password).</param>
        /// <returns>
        /// Una estructura <see cref="Reply"/> que contiene el token JWT y la información básica del usuario 
        /// si la autenticación es exitosa.
        /// </returns>
        /// <response code="200">Retorna el token y los datos del usuario.</response>
        /// <response code="401">Si las credenciales son inválidas o el usuario no existe.</response>
        /// <response code="400">Si hay un error en el formato de los datos enviados.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.Authenticate(loginDto.Email, loginDto.Password);

            if (result == null || !result.Flag)
            {
                return Unauthorized(new Reply
                {
                    Ok = false,
                    Message = result?.Message ?? "Credenciales incorrectas o servicio no disponible",
                    Data = null
                });
            }

            var token = result.Data != null ? _authService.GenerateToken((UserModels)result.Data) : null;

            oReply.Data = new
            {
                token = token,
                user = result.Data
            };
            oReply.Ok = result.Flag;
            oReply.Message = result.Message;

            return result.Status switch
            {
                200 => Ok(oReply),
                400 => BadRequest(oReply),
                404 => NotFound(oReply),
                _ => StatusCode(result.Status, oReply)
            };
        }
        #endregion

        #region Registra un nuevo usuario en el sistema.
        /// POST: api/Auth/register
        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <remarks>
        /// Método que registra un nuevo usuario en el sistema.
        /// </remarks>
        /// <param name="registerDto">Datos del nuevo usuario.</param>
        /// <returns>Respuesta estandarizada con el resultado de la creación.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Reply), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);

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

        #region Actualiza la información del usuario autenticado.
        /// POST: api/Auth/register
        /// <summary>
        /// Actualiza la información del usuario autenticado.
        /// </summary>
        /// <remarks>
        /// Método que actualiza la información del usuario autenticado.
        /// </remarks>
        /// <param name="updateDto">Datos para actualizar un usuario.</param>
        /// <returns>Respuesta estandarizada con el resultado de la creación.</returns>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el token JWT de acceso</response>
        /// <response code="400">Bad Request. Url no encontrada o formato de parametro incorrecto</response>
        /// <response code="200">OK. lista retornada con éxito</response>
        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto updateDto)
        {
            // 1. Depuración: Ver si el usuario está llegando autenticado
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Unauthorized(new Reply { Ok = false, Message = "No estás autenticado" });
            }

            // 2. Extraer el ID (ClaimTypes.NameIdentifier suele mapearse a "sub" o al ID que guardaste)
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                userIdClaim = User.FindFirst("id");
            }

            if (userIdClaim == null)
                return BadRequest(new Reply { Ok = false, Message = "No se pudo identificar al usuario en el token" });

            int userId = int.Parse(userIdClaim.Value);

            // 3. Llamar al servicio
            var result = await _authService.UpdateUser(userId, updateDto);

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