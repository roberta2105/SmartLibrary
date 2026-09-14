using CleanArchMvc_V2.Domain.Validation;

namespace SmartLibrary.Domain.Entities
{
    public sealed class Livro : EntidadeBase
    {
        public string Titulo { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public string Autor { get; private set; } = string.Empty;
        public DateTime DataPublicacao { get; private set; }
        public DateTime? DataExclusao { get; private set; }
        public string Isbn { get; private set; } = string.Empty;
        public int QuantidadeTotal { get; private set; } = 0;
        public int QuantidadeDisponivel { get; private set; } = 0;
        public int CategoriaId { get; private set; } = 0;
        public Categoria Categoria { get; private set; }
        public ICollection<Emprestimo> Emprestimos { get; private set; } = new List<Emprestimo>();


        public Livro(
            string titulo, 
            string descricao, 
            string autor, 
            DateTime dataPublicacao, 
            string isbn, 
            int categoriaId, 
            int quantidadeTotal)
        {
            ValidateDomain(
                titulo,
                descricao,
                autor,
                dataPublicacao,
                isbn,
                categoriaId,
                quantidadeTotal
                );

            Titulo = titulo;
            Descricao = descricao;
            Autor = autor;
            DataPublicacao = dataPublicacao;
            Isbn = isbn; 
            QuantidadeTotal = quantidadeTotal;
            QuantidadeDisponivel = quantidadeTotal;
            DataCriacao = DateTime.UtcNow;
            CategoriaId = categoriaId;
        }

        public void Update(
            string titulo, 
            string descricao, 
            string autor, 
            DateTime dataPublicacao, 
            string isbn, 
            int categoriaId, 
            int quantidadeTotal)
        {
            ValidateDomain(
                titulo,
                descricao,
                autor,
                dataPublicacao,
                isbn,
                categoriaId,
                quantidadeTotal);


            var quantidadeEmprestada = QuantidadeTotal - QuantidadeDisponivel;

            DomainExceptionValidation.When(
                quantidadeTotal < quantidadeEmprestada,
                "A quantidade total não pode ser menor do que a quantidade de livros emprestados.");

            Titulo = titulo;
            Descricao = descricao;
            Autor = autor;
            DataPublicacao = dataPublicacao;
            Isbn = isbn;
            QuantidadeTotal = quantidadeTotal;
            QuantidadeDisponivel = quantidadeTotal - quantidadeEmprestada;
            CategoriaId = categoriaId;
        }

        public void Deactivate()
        {
            DomainExceptionValidation.When(DataExclusao.HasValue,
                "O livro já foi inativado.");

            DataExclusao = DateTime.UtcNow;
        }

        public void Emprestar()
        {
            DomainExceptionValidation.When(
                QuantidadeDisponivel <= 0,
                "Não há livros disponíveis para empréstimo.");

            QuantidadeDisponivel--;
        }

        public void Devolver()
        {
            DomainExceptionValidation.When(
                QuantidadeDisponivel >= QuantidadeTotal,
                "A quantidade disponível já corresponde à quantidade total.");

            QuantidadeDisponivel++;
        }

        private void ValidateDomain(
            string titulo,
            string descricao,
            string autor,
            DateTime dataPublicacao,
            string isbn,
            int categoriaId,
            int quantidadeTotal
            )
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(titulo),
                "O título é obrigatório.");

            DomainExceptionValidation.When(titulo.Length > 50,
                "O título deve possuir no máximo 50 caracteres.");

            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(descricao),
                "A descrição é obrigatória.");

            DomainExceptionValidation.When(descricao.Length > 200,
                "A descrição deve possuir no máximo 200 caracteres.");

            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(autor),
                "O autor é obrigatório.");

            DomainExceptionValidation.When(autor.Length > 100,
                "O autor deve possuir no máximo 100 caracteres.");

            DomainExceptionValidation.When(dataPublicacao > DateTime.Today,
                "A data de publicação não pode ser futura.");

            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(isbn),
                "O ISBN é obrigatório.");

            DomainExceptionValidation.When(isbn.Length != 13,
                "O ISBN deve possuir 13 dígitos.");

            DomainExceptionValidation.When(categoriaId <= 0,
                "O ID da categoria é inválido.");

            DomainExceptionValidation.When(quantidadeTotal < 0,
                "A quantidade total deve ser maior do que zero.");
        }
    }
}
