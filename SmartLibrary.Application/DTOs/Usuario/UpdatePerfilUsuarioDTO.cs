using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Usuario;

public class UpdatePerfilUsuarioDTO
{
    [Required(ErrorMessage = "O perfil é obrigatório!")]
    public string Perfil { get; set; } = string.Empty;
}
