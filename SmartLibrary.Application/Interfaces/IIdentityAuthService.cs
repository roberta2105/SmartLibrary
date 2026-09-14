using SmartLibrary.Application.DTOs.Usuario;

namespace SmartLibrary.Application.Interfaces;

public interface IIdentityAuthService
{
    Task<TokenUsuarioDTO?> AuthenticateAsync(
        string email,
        string senha);
}
