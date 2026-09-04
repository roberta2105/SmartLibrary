using CleanArchMvc_V2.Domain.Validation;

namespace SmartLibrary.Domain.Entities
{
    public sealed class Categoria : EntidadeBase
    {
        public string Nome { get; private set; } = string.Empty;
        public ICollection<Livro> Livros { get; private set; } = new List<Livro>();

        public Categoria(string nome)
        {
            ValidateDomain(nome);
            DataCriacao = DateTime.UtcNow;
        }

        public void Update(string nome)
        {
            ValidateDomain(nome);
        }

        private void ValidateDomain(string nome)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(nome),
                "O nome é obrigatório.");

            DomainExceptionValidation.When(nome.Length > 50,
               "O nome deve possuir no máximo 50 caracteres.");

            Nome = nome;
        }
    }
}
