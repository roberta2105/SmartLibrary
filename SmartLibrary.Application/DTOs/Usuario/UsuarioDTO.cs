using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.DTOs.Usuario;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public PerfilUsuario Perfil { get; set; } = PerfilUsuario.Usuario;
    public IEnumerable<EmprestimoDTO> Emprestimos { get; set; }= new List<EmprestimoDTO>();
}
