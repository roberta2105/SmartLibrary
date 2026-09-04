using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Categoria;

public class CreateCategoriaDTO
{
    [Required(ErrorMessage = "O nome é obrigatório!")]
    [MaxLength(20)]
    public string Nome { get; set; } = string.Empty;
}
