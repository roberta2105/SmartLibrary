using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infra.Data.EntitiesConfiguration;

public class EmprestimoConfiguration : IEntityTypeConfiguration<Emprestimo>
{
    public void Configure(EntityTypeBuilder<Emprestimo> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.StatusEmprestimo)
            .IsRequired();

        builder.Property(x => x.DataDevolucaoPrevista)
            .IsRequired();

        builder.Property(x => x.DataDevolucaoEfetiva)
            .IsRequired(false);

        builder.Property(x => x.QuantidadeRenovacoes)
            .IsRequired();

        builder.HasOne(x => x.Livro)
            .WithMany(x => x.Emprestimos)
            .HasForeignKey(x => x.LivroId);

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.Emprestimos)
            .HasForeignKey(x => x.UsuarioId);
    }
}
