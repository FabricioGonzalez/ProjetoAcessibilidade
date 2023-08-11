using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqliteDatasource.Internals.Dto;

namespace SqliteDatasource.Internals.EntityConfigurations;

public class CidadeEntityConfiguration : IEntityTypeConfiguration<CidadeDto>
{
    public void Configure(
        EntityTypeBuilder<CidadeDto> builder
    )
    {
        builder.HasKey(key => key.CodigoIbge);

        builder.Property(prop => prop.CodigoIbge).HasColumnName("codigo_ibge");
        builder.Property(prop => prop.Nome).HasColumnName("nome");
        builder.Property(prop => prop.Latitude).HasColumnName("latitude");
        builder.Property(prop => prop.Longitude).HasColumnName("longitude");
        builder.Property(prop => prop.Capital).HasColumnName("capital");
        builder.Property(prop => prop.CodigoUf).HasColumnName("codigo_uf");
        builder.Property(prop => prop.SiafiId).HasColumnName("siafi_id");
        builder.Property(prop => prop.Ddd).HasColumnName("ddd");
        builder.Property(prop => prop.FusoHorario).HasColumnName("fuso_horario");

        builder.HasOne(prop => prop.Uf)
            .WithMany(prop => prop.Cidades)
            .HasForeignKey(key => key.CodigoUf);

        builder.ToTable("Cidades");
    }
}