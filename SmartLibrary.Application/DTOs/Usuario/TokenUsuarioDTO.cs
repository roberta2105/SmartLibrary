namespace SmartLibrary.Application.DTOs.Usuario;

public class TokenUsuarioDTO
{
    public string Token { get; set; } = string.Empty; 
    public string UsuarioId { get; set; } = string.Empty; 
    public string Nome { get; set; } = string.Empty; 
    public string Email { get; set; } = string.Empty; 
    public IEnumerable<string> Perfis { get; set; } = [];
}
