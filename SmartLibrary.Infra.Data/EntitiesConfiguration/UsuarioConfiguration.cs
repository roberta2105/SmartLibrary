using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Infra.Data.EntitiesConfiguration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(p => p.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Email)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Perfil)
            .IsRequired();

        builder.Property(p => p.Cpf)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(p => p.Telefone)
            .HasMaxLength(13)
            .IsRequired();

        builder.Property(x => x.DataExclusao)
            .IsRequired(false);

        builder.HasQueryFilter(x => x.DataExclusao == null);
    }
}
