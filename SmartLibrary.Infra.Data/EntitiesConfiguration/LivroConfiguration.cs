using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infra.Data.EntitiesConfiguration;

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(p => p.Titulo)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Descricao)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.Autor)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Isbn)
            .HasMaxLength(13)
            .IsRequired();
        builder.HasIndex(l => l.Isbn)
            .IsUnique();

        builder.Property(x => x.QuantidadeTotal)
       .IsRequired();

        builder.Property(x => x.QuantidadeDisponivel)
            .IsRequired();

        builder.Property(x => x.DataPublicacao)
            .IsRequired();

        builder.Property(x => x.DataExclusao)
            .IsRequired(false);

        builder.HasQueryFilter(x => x.DataExclusao == null);

        builder.HasOne(e => e.Categoria)
            .WithMany(e => e.Livros)
            .HasForeignKey(e => e.CategoriaId);
    }

}
