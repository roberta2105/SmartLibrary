using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Interfaces;

public interface IEmprestimoRepository : IRepository<Emprestimo>
{
    Task<bool> HasEmprestimoUsuarioAsync(
        int usuarioId,
        int? livroId,
        StatusEmprestimo? statusEmprestimo = null,
        bool? estaAtrasado = null
    );

    Task<bool> HasLimiteEmprestimoUsuarioAsync(
        int usuarioId
    );

    Task<IEnumerable<Emprestimo>> GetAllAsync(
        int? usuarioId = null,
        string? nomeUsuario = null,
        int? livroId = null,
        string? nomeLivro = null,
        int? quantidadeRenovacoes = null,
        bool? estaAtrasado = null,
        StatusEmprestimo? statusEmprestimo = null,
        DateTime? dataDevolucaoPrevista = null,
        DateTime? dataDevolucaoEfetiva = null
    );
}
