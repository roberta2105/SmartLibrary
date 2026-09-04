using SmartLibrary.Application.DTOs.Livro;

namespace SmartLibrary.Application.Interfaces;

public interface ILivroService : IService<LivroDTO, CreateLivroDTO, UpdateLivroDTO>
{
    Task<IEnumerable<LivroDTO>> GetAll(
        string? titulo = null,
        string? descricao = null,
        string? autor = null,
        DateTime? dataPublicacao = null,
        string? isbn = null,
        int? categoriaId = null,
        string? categoriaNome = null
    );
    Task<LivroDTO?> Deactivate(int id);
}
