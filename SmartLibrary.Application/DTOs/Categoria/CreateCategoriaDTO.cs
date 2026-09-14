using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Categoria;

public class CreateCategoriaDTO
{
    [Required(ErrorMessage = "O nome é obrigatório!")]
    [MaxLength(20, ErrorMessage = "O nome deve possuir no máximo 20 caracteres")]
    public string Nome { get; set; } = string.Empty;    
}
