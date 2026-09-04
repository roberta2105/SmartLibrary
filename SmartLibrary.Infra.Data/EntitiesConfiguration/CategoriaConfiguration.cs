using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infra.Data.EntitiesConfiguration;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(p => p.Nome).HasMaxLength(50).IsRequired();

        builder.HasData(
            new
            {
                Id = 1,
                Nome = "Romance",
                DataCriacao = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 2,
                Nome = "Terror",
                DataCriacao = new DateTime(2026, 1, 1)
            },
            new
            {
                Id = 3,
                Nome = "Fantasia",
                DataCriacao = new DateTime(2026, 1, 1)
            }
        );
    }

}
