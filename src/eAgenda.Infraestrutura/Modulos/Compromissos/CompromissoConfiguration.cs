using eAgenda.Dominio.Modulos.Compromissos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Modulos.Compromissos;

public sealed class CompromissoConfiguration : IEntityTypeConfiguration<Compromisso>
{
    public void Configure(EntityTypeBuilder<Compromisso> builder)
    {
        builder.ToTable("TBCompromissos");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Assunto)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.DataOcorrencia)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(c => c.HoraInicio)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(c => c.HoraTermino)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(c => c.TipoCompromisso)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(c => c.LinkOuLocal)
            .IsRequired();

        builder.HasMany(c => c.Contatos)
            .WithMany()
            .UsingEntity(j => j.ToTable("TBCompromissos_Contato"));

    }
}
