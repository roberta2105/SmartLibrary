using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Interfaces;

public interface IUsuarioService : IService<UsuarioDTO, CreateUsuarioDTO, UpdateUsuarioDTO>
{
    Task<IEnumerable<UsuarioDTO>> GetAll(
         string? nome = null,
         string? email = null,
         string? cpf = null,
         string? telefone = null,
         PerfilUsuario? perfil = null
    );
    Task<UsuarioDTO?> Deactivate(int id);
}
