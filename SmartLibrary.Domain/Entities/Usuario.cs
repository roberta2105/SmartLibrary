using CleanArchMvc_V2.Domain.Validation;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.Domain.Entities;

public sealed class Usuario : EntidadeBase
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public PerfilUsuario Perfil { get; private set; } = PerfilUsuario.Usuario;
    public DateTime? DataExclusao { get; private set; }
    public ICollection<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();


    public Usuario(string nome, string email, string cpf, string telefone, PerfilUsuario perfil = PerfilUsuario.Usuario)
    {
        ValidateDomain(nome, email, cpf, telefone, perfil);
        DataCriacao = DateTime.UtcNow;
    }

    public void Update(string nome, string email, string cpf, string telefone, PerfilUsuario perfil = PerfilUsuario.Usuario)
    {
        ValidateDomain(nome, email, cpf, telefone, perfil);
    }

    public void Deactivate()
    {
        DomainExceptionValidation.When(DataExclusao.HasValue,
            "O usuário já foi inativado");

        DataExclusao = DateTime.UtcNow;
    }

    private void ValidateDomain(string nome, string email, string cpf, string telefone, PerfilUsuario perfil = PerfilUsuario.Usuario)
    {
        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(nome), 
            "O nome é obrigatório");

        DomainExceptionValidation.When(nome.Length > 100,
            "O nome deve conter até 100 caracteres");

        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(email),
            "O e-mail é obrigatório");

        DomainExceptionValidation.When(email.Length > 100,
            "O e-mail deve conter até 100 caracteres");

        DomainExceptionValidation.When(!email.Contains("@"),
            "E-mail inválido");

        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(cpf),
            "O CPF é obrigatório");

        DomainExceptionValidation.When(cpf.Length != 11 || !cpf.All(char.IsDigit),
            "O CPF deve possuir 11 dígitos");

        DomainExceptionValidation.When(string.IsNullOrWhiteSpace(telefone),
            "O telefone é obrigatório");

        DomainExceptionValidation.When(!Enum.IsDefined(typeof(PerfilUsuario), perfil),
            "Perfil de usuário inválido");

        Nome = nome;
        Email = email;
        Cpf = cpf;
        Telefone = telefone;
        Perfil = perfil;
    }
}
