using Microsoft.EntityFrameworkCore;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Domain.Interfaces;
using SmartLibrary.Infra.Data.Context;

namespace SmartLibrary.Infra.Data.Repositories;

public class LivroRepository : Repository<Livro>, ILivroRepository
{
    private readonly ApplicationDbContext _context;

    public LivroRepository(ApplicationDbContext context) : base(context)
    { 
        _context = context;
    }

    public async Task<IEnumerable<Livro>> GetAllAsync(
        string? titulo = null,
        string? descricao = null,
        string? autor = null,
        DateTime? dataPublicacao = null,
        string? isbn = null,
        int? categoriaId = null,
        string? categoriaNome = null)
    {
        var query = _context.Livros
           .Include(x => x.Categoria)
           .AsQueryable();

        if (categoriaId.HasValue)
            query = query.Where(x => x.CategoriaId == categoriaId.Value);

        if (!string.IsNullOrWhiteSpace(categoriaNome))
            query = query.Where(x => x.Categoria.Nome.Contains(categoriaNome));

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(x => x.Titulo.Contains(titulo));

        if (!string.IsNullOrWhiteSpace(descricao))
            query = query.Where(x => x.Descricao.Contains(descricao));

        if (!string.IsNullOrWhiteSpace(autor))
            query = query.Where(x => x.Autor.Contains(autor));

        if (!string.IsNullOrWhiteSpace(isbn))
            query = query.Where(x => x.Isbn.Contains(isbn));

        if (dataPublicacao.HasValue)
            query = query.Where(x =>
                x.DataPublicacao.Date ==
                dataPublicacao.Value.Date);

        return await query.ToListAsync();
    }

    public async Task<Livro?> GetByIdAsync(int id)
    {
        return await _context.Livros
            .Include(x => x.Categoria)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Livro?> GetByIsbnAsync(string isbn)
    {
        return await _context.Livros.FirstOrDefaultAsync(l => l.Isbn == isbn);
    }

    public async Task<bool> HasActiveEmprestimoAsync(int livroId)
    {
        return await _context.Emprestimos
            .AnyAsync(e =>
            e.LivroId == livroId &&
            e.StatusEmprestimo == StatusEmprestimo.Emprestado);
    }
}

