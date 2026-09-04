using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Interfaces;

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<IEnumerable<Categoria>> GetAllAsync();
    Task<Categoria> RemoveAsync(Categoria categoriaEntity);
    Task<bool> HasActiveLivrosAsync(int categoriaId);
}
