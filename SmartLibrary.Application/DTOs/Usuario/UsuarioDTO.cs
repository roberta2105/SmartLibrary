namespace SmartLibrary.Application.DTOs.Usuario;

public class UsuarioDTO
{
    public string Id { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Perfil { get; set; } = "Usuario";
    public DateTime DataCriacao { get; set; }
    public DateTime? DataExclusao { get; set; }
}
