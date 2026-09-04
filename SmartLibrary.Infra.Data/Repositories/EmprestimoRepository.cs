using Microsoft.EntityFrameworkCore;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Domain.Interfaces;
using SmartLibrary.Infra.Data.Context;

namespace SmartLibrary.Infra.Data.Repositories;

public class EmprestimoRepository : Repository<Emprestimo>, IEmprestimoRepository
{
    private readonly ApplicationDbContext _context;

    public EmprestimoRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> HasLimiteEmprestimoUsuarioAsync(int usuarioId)
    {
        var quantidadeEmprestimos = await _context.Emprestimos
            .CountAsync(
                  e => e.UsuarioId == usuarioId &&
                  e.StatusEmprestimo == StatusEmprestimo.Emprestado
            );

        return quantidadeEmprestimos >= 5;
    }

    public async Task<bool> HasEmprestimoUsuarioAsync(
         int usuarioId,
         int? livroId,
         StatusEmprestimo? statusEmprestimo = null,
         bool? estaAtrasado = null)
    {
        var query = _context.Emprestimos
            .Where(e => e.UsuarioId == usuarioId);

        if (livroId != null)
        {
            query = query.Where(e => e.LivroId == livroId);
        }

        if (statusEmprestimo.HasValue)
        {
            query = query.Where(e =>
                e.StatusEmprestimo == statusEmprestimo.Value);
        }

        if (estaAtrasado.HasValue)
        {
            if (estaAtrasado.Value)
            {
                query = query.Where(e =>
                    e.StatusEmprestimo == StatusEmprestimo.Emprestado &&
                    e.DataDevolucaoPrevista < DateTime.Now);
            }
            else
            {
                query = query.Where(e =>
                    e.StatusEmprestimo != StatusEmprestimo.Emprestado ||
                    e.DataDevolucaoPrevista >= DateTime.Now);
            }
        }

        return await query.AnyAsync();
    }


    public async Task<IEnumerable<Emprestimo>> GetAllAsync(
            int? usuarioId = null,
            string? nomeUsuario = null,
            int? livroId = null, string?
            nomeLivro = null, int?
            quantidadeRenovacoes = null,
            bool? estaAtrasado = null,
            StatusEmprestimo? statusEmprestimo = null,
            DateTime? dataDevolucaoPrevista = null,
            DateTime? dataDevolucaoEfetiva = null
        )
    {
        var query = _context.Emprestimos
            .Include(x => x.Livro)
            .Include(x => x.Usuario)
            .AsQueryable();

        if (usuarioId.HasValue)
            query = query.Where(x => x.UsuarioId == usuarioId.Value);

        if (livroId.HasValue)
            query = query.Where(x => x.LivroId == livroId.Value);

        if (!string.IsNullOrWhiteSpace(nomeLivro))
            query = query.Where(x => x.Livro.Titulo.Contains(nomeLivro));

        if (!string.IsNullOrWhiteSpace(nomeUsuario))
            query = query.Where(x => x.Usuario.Nome.Contains(nomeUsuario));

        if (quantidadeRenovacoes.HasValue)
            query = query.Where(x =>
                x.QuantidadeRenovacoes == quantidadeRenovacoes.Value);

        if (estaAtrasado.HasValue)
        {
            if (estaAtrasado.Value)
            {
                query = query.Where(x =>
                    x.StatusEmprestimo == StatusEmprestimo.Emprestado &&
                    x.DataDevolucaoPrevista < DateTime.Now);
            }
            else
            {
                query = query.Where(x =>
                    x.StatusEmprestimo != StatusEmprestimo.Emprestado ||
                    x.DataDevolucaoPrevista >= DateTime.Now);
            }
        }

        if (statusEmprestimo.HasValue)
            query = query.Where(x =>
                x.StatusEmprestimo == statusEmprestimo.Value);

        if (dataDevolucaoPrevista.HasValue)
            query = query.Where(x =>
                x.DataDevolucaoPrevista.Date ==
                dataDevolucaoPrevista.Value.Date);

        if (dataDevolucaoEfetiva.HasValue)
            query = query.Where(x =>
                x.DataDevolucaoEfetiva.HasValue &&
                x.DataDevolucaoEfetiva.Value.Date ==
                dataDevolucaoEfetiva.Value.Date);

        return await query.ToListAsync();
    }

 
}

