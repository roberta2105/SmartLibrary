using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<IEnumerable<Usuario>> GetAllAsync(
        string? nome = null,
        string? email = null,
        string? cpf = null,
        string? telefone = null,
        PerfilUsuario? perfil = null
    );

    Task<Usuario?> GetByCpfAsync(string cpf);
    Task<Usuario?> GetByEmailAsync(string email);
}
