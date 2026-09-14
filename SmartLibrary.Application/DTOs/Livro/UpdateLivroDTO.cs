using SmartLibrary.Application.DTOs.Categoria;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartLibrary.Application.DTOs.Livro;

public class UpdateLivroDTO
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório!")]
    [MaxLength(50, ErrorMessage = "O título deve ter no máximo 50 caracteres.")]
    public string Titulo { get; set; } = string.Empty;


    [Required(ErrorMessage = "A descrição é obrigatória!")]
    [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;


    [Required(ErrorMessage = "O autor é obrigatório!")]
    [MaxLength(100, ErrorMessage = "O autor deve ter no máximo 100 caracteres.")]
    public string Autor { get; set; } = string.Empty;


    [Required(ErrorMessage = "O ISBN é obrigatório!")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "O ISBN deve conter exatamente 13 dígitos.")]
    public string Isbn { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de publicação é obrigatória!")]
    public DateTime DataPublicacao { get; set; }

    [JsonIgnore]
    public CategoriaDTO? Categoria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A categoria é obrigatória!")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "A quantidade total é obrigatória!")]
    [Range(0, int.MaxValue, ErrorMessage = "A quantidade total não pode ser negativa.")]
    public int QuantidadeTotal { get; set; }
}
