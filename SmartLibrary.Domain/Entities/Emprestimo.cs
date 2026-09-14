using CleanArchMvc_V2.Domain.Validation;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public sealed class Emprestimo : EntidadeBase
{
    public StatusEmprestimo StatusEmprestimo { get; private set; }
    public DateTime DataDevolucaoPrevista { get; private set; }
    public DateTime? DataDevolucaoEfetiva { get; private set; }
    public int QuantidadeRenovacoes { get; private set; } = 0;
    public int LivroId { get; private set; }
    public Livro Livro { get; private set; }
    public string UsuarioId { get; private set; }
    public bool EstaAtrasado =>
      StatusEmprestimo == StatusEmprestimo.Emprestado &&
      DataDevolucaoPrevista < DateTime.UtcNow.Date;


    public Emprestimo(int livroId, string usuarioId)
    {
        ValidateDomain(livroId, usuarioId);

        LivroId = livroId;
        UsuarioId = usuarioId;

        DataDevolucaoPrevista = DateTime.UtcNow.AddDays(15);
        StatusEmprestimo = StatusEmprestimo.Emprestado;
        DataCriacao = DateTime.UtcNow;
    }

    public void Renovar()
    {
        DomainExceptionValidation.When(
            QuantidadeRenovacoes >= 1,
            "O empréstimo já foi renovado. Não é possível aumentar o prazo novamente.");

        DomainExceptionValidation.When(
            DataDevolucaoEfetiva.HasValue,
            "Não é possível renovar um empréstimo já devolvido.");

        DomainExceptionValidation.When(
           EstaAtrasado,
           "Não é possível renovar um empréstimo atrasado.");

        DataDevolucaoPrevista = DataDevolucaoPrevista.AddDays(15);
        QuantidadeRenovacoes++;
    }

    public void Devolver()
    {
        DomainExceptionValidation.When(
            DataDevolucaoEfetiva.HasValue,
            "O livro já foi devolvido.");

        DataDevolucaoEfetiva = DateTime.UtcNow;
        StatusEmprestimo = StatusEmprestimo.Devolvido;
    }


    private void ValidateDomain(int livroId, string usuarioId)
    {
        DomainExceptionValidation.When(livroId <= 0,
            "Livro inválido.");

        DomainExceptionValidation.When(string.IsNullOrEmpty(usuarioId),
            "Usuário inválido.");
    }
}
