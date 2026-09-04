using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Domain.Interfaces;

public interface ILivroRepository : IRepository<Livro>
{
    Task<IEnumerable<Livro>> GetAllAsync(
        string? titulo = null,
        string? descricao = null,
        string? autor = null,
        DateTime? dataPublicacao = null,
        string? isbn = null,
        int? categoriaId = null,
        string? categoriaNome = null
    );
    Task<Livro?> GetByIsbnAsync(string isbn);
    Task<bool> HasActiveEmprestimoAsync(int livroId);
}
