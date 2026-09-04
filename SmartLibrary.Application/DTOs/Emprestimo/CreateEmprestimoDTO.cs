using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs;

public class CreateEmprestimoDTO
{
    [Range(1, int.MaxValue, ErrorMessage = "Livro inválido.")]
    public int LivroId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Usuário inválido.")]
    public int UsuarioId { get; set; }
}
