using SmartLibrary.Application.DTOs.Categoria;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartLibrary.Application.DTOs.Livro;

public class CreateLivroDTO
{
    [Required(ErrorMessage = "O título é obrigatório!")] 
    [MaxLength(50)]
    public string Titulo { get; set; } = string.Empty;


    [Required(ErrorMessage = "A descrição é obrigatória!")]
    [MaxLength(200)]
    public string Descricao { get; set; } = string.Empty;


    [Required(ErrorMessage = "O autor é obrigatório!")]
    [MaxLength(100)]
    public string Autor { get; set; } = string.Empty;


    [Required(ErrorMessage = "O ISBN é obrigatório!")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "O ISBN deve conter exatamente 13 dígitos.")]
    public string Isbn { get; set; } = string.Empty;

    public DateTime DataPublicacao { get; set; }

    [JsonIgnore]
    public CategoriaDTO? Categoria { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "A categoria é obrigatória!")]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "A quantidade total é obrigatória!")]
    //[Range(1, 1000, ErrorMessage = "A quantidade total deve estar entre 1 e 1000.")]
    public int QuantidadeTotal { get; set; }
}
