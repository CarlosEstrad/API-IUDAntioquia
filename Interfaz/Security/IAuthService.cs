using MusicPlaylist.Api.DTOs.Helpers;
using MusicPlaylist.Api.DTOs.Login;
using MusicPlaylist.Api.Models.Users;

namespace MusicPlaylist.Api.Interfaz.Security
{
    public interface IAuthService
    {
        string GenerateToken(UserModels user);
        Task<LoginReplyModel?> Authenticate(string email, string password);
        Task<LoginReplyModel> Register(RegisterDto registerDto);
        Task<LoginReplyModel> UpdateUser(int userId, UserUpdateDto updateDto);
    }
}
