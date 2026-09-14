using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Infra.Data.Identity;

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

        builder.Property(e => e.UsuarioId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(e => e.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Livro)
            .WithMany(l => l.Emprestimos)
            .HasForeignKey(e => e.LivroId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
