using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Interfaces;

public interface IEmprestimoRepository : IRepository<Emprestimo>
{
    Task<IEnumerable<Emprestimo>> GetAllAsync(
        string? usuarioId = null,
        string? nomeUsuario = null,
        int? livroId = null,
        string? nomeLivro = null,
        int? quantidadeRenovacoes = null,
        bool? estaAtrasado = null,
        StatusEmprestimo? statusEmprestimo = null,
        DateTime? dataDevolucaoPrevista = null,
        DateTime? dataDevolucaoEfetiva = null
    );
    Task<Emprestimo?> GetByIdAsync(int id);

    Task<bool> HasEmprestimoUsuarioAsync(
       string usuarioId,
       int? livroId,
       StatusEmprestimo? statusEmprestimo = null,
       bool? estaAtrasado = null
   );

    Task<bool> HasLimiteEmprestimoUsuarioAsync(
        string usuarioId
    );
}
