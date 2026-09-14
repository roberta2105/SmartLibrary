using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs;

public class CreateEmprestimoDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Livro inválido.")]
    public int LivroId { get; set; }

    [Required(ErrorMessage = "Usuário inválido.")]
    public string UsuarioId { get; set; } = string.Empty;
}
