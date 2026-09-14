using SmartLibrary.Application.DTOs.Usuario;

namespace SmartLibrary.Application.Interfaces;

public interface IIdentityService
{
    Task<IEnumerable<UsuarioDTO>> GetAll(
        string? nome = null,
        string? email = null,
        string? cpf = null,
        string? telefone = null,
        string? perfil = null);

    Task<UsuarioDTO?> GetById(string id);

    Task Add(CreateUsuarioDTO usuarioDto);

    Task Update(string id, UpdateUsuarioDTO usuarioDto);

    Task UpdatePerfil(string id, string novoPerfil);

    Task<UsuarioDTO?> Deactivate(string id);
}