using Microsoft.EntityFrameworkCore;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Interfaces;
using SmartLibrary.Infra.Data.Context;

namespace SmartLibrary.Infra.Data.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    private readonly ApplicationDbContext _context;

    public CategoriaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Categoria>> GetAllAsync()
    {
        return await _context.Categorias.ToListAsync();
    }

    public async Task<Categoria?> GetByIdAsync(int id)
    {
        return await _context.Categorias
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> HasActiveCategoryWithSameNameAsync(string categoryName, int? categoriaId)
    {
        return await _context.Categorias
            .AnyAsync(c => 
                c.Nome == categoryName &&
                c.Id != categoriaId
            );

    }

    public async Task<bool> HasActiveLivrosAsync(int categoriaId)
    {
        return await _context.Livros
            .AnyAsync(e =>
            e.CategoriaId == categoriaId);
    }

    public async Task<Categoria> RemoveAsync(Categoria categoriaEntity)
    {
        _context.Remove(categoriaEntity);

        return categoriaEntity;
    }
}

