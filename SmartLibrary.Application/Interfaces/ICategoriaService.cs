using SmartLibrary.Application.DTOs.Categoria;

namespace SmartLibrary.Application.Interfaces;

public interface ICategoriaService : IService<CategoriaDTO, CreateCategoriaDTO, UpdateCategoriaDTO>
{
    Task<IEnumerable<CategoriaDTO>> GetAll();
    Task Remove(int id);
}
