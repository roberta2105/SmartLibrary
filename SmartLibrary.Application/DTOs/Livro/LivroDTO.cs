using SmartLibrary.Application.DTOs.Categoria;

namespace SmartLibrary.Application.DTOs.Livro;

public class LivroDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public DateTime DataPublicacao { get; set; }
    public int CategoriaId { get; set; }
    public CategoriaDTO? Categoria { get; set; }
    public int QuantidadeTotal { get; set; } = 0;
}
