using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicPlaylist.Api.Data;
using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.Login;
using MusicPlaylist.Api.Interfaz.Security;
using MusicPlaylist.Api.Models.Users;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicPlaylist.Api.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly LoginReplyModel reply = new LoginReplyModel();

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        #region Valida las credenciales de un usuario contra la base de datos.

        public async Task<LoginReplyModel?> Authenticate(string email, string password)
        {
            // Buscamos el usuario por email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            bool isValid = false;

            // Verificamos si la contraseña coincide con el Hash guardado
            if (user != null)
                isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            reply.Data = user;
            reply.Message = user == null ? "El usuario no existe" : isValid ? "Usuario Correcto" : "Contraseña incorrecta";
            reply.Flag = true;
            reply.Status = 200;
            return reply;

        }
        #endregion

        #region Genera un token JWT (JSON Web Token) firmado para un usuario autenticado.

        public string GenerateToken(UserModels user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Los "Claims" son la información que viaja dentro del token
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8), // El token dura 8 horas
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Registra un nuevo usuario en el sistema.
        public async Task<LoginReplyModel> Register(RegisterDto registerDto)
        {
            // 1. Verificar si el correo o el nombre de usuario ya existe
            var existingUser = await _context.Users.AnyAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username);
            if (existingUser)
            {
                return new LoginReplyModel
                {
                    Status = 400,
                    Flag = false,
                    Message = "El correo electrónico o el Nombre de usuario ya está registrado"
                };
            }

            // 2. Crear la entidad y Encriptar contraseña
            var user = new UserModels
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new LoginReplyModel
                {
                    Status = 200,
                    Flag = true,
                    Message = "Usuario creado exitosamente",
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return new LoginReplyModel
                {
                    Status = 500,
                    Flag = false,
                    Message = "Error interno al crear el usuario: " + ex.Message
                };
            }
        }
        #endregion

        #region Actualiza la información del usuario autenticado.
        public async Task<LoginReplyModel> UpdateUser(int userId, UserUpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return new LoginReplyModel { Status = 404, Flag = false, Message = "Usuario no encontrado" };
            }

            // 1. Verificar si el nombre de usuario ya existe
            var existingUser = await _context.Users.AnyAsync(u => u.Username == updateDto.Username);
            if (existingUser)
            {
                return new LoginReplyModel
                {
                    Status = 400,
                    Flag = false,
                    Message = "El nombre de usuario ya está registrado"
                };
            }

            // Actualizar nombre de usuario
            user.Username = updateDto.Username;

            // Si envió una nueva contraseña, la hasheamos
            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.Password);
            }

            try
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new LoginReplyModel
                {
                    Status = 200,
                    Flag = true,
                    Message = "Perfil actualizado correctamente",
                    Data = new
                    {
                        id = user.Id,
                        username = user.Username,
                        email =  user.Email,
                    }
                };
            }
            catch (Exception ex)
            {
                return new LoginReplyModel { Status = 500, Flag = false, Message = "Error: " + ex.Message };
            }
        }
        #endregion
    }
}
