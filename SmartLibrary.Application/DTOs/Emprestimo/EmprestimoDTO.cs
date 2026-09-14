using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Application.DTOs;

public class EmprestimoDTO
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime DataDevolucaoPrevista { get; set; }
    public DateTime? DataDevolucaoEfetiva { get; set; }
    public int QuantidadeRenovacoes { get; set; }
    public bool EstaAtrasado { get; set; }
    public StatusEmprestimo StatusEmprestimo { get; set; }
    public string StatusEmprestimoLabel { get; set; } = string.Empty;
}
