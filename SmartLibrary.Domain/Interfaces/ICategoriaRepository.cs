using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Domain.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria> RemoveAsync(Categoria categoriaEntity);
    Task<Categoria?> GetByIdAsync(int id);
    Task<bool> HasActiveLivrosAsync(int categoriaId);
    Task<bool> HasActiveCategoryWithSameNameAsync(string categoryName, int? categoriaId = null);
}
