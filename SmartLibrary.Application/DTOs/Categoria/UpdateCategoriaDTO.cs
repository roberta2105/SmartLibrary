using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Categoria;

public class UpdateCategoriaDTO
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório!")]
    [MaxLength(20)]
    public string Nome { get; set; } = string.Empty;
}
