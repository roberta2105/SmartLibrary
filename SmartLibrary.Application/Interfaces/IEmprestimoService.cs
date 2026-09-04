using SmartLibrary.Application.DTOs;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.Interfaces;

public interface IEmprestimoService
{
    Task<EmprestimoDTO> GetById(int id);
    Task Add(CreateEmprestimoDTO emprestimoDto);
    Task<EmprestimoDTO?> Renovar(int id);
    Task<EmprestimoDTO?> Devolver(int id);
    Task<IEnumerable<EmprestimoDTO>> GetAll(
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
