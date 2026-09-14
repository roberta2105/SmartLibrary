using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Usuario;

public class LoginUsuarioDTO
{
    [Required(ErrorMessage = "O email é obrigatório!")]
    [EmailAddress(ErrorMessage = "O email informado é inválido.")] 
    public string Email { get; set; } = string.Empty; 
    
    [Required(ErrorMessage = "A senha é obrigatória!")] 
    public string Senha { get; set; } = string.Empty;
}
