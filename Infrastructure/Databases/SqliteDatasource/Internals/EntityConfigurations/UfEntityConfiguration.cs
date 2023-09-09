using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SqliteDatasource.Internals.Dto;

namespace SqliteDatasource.Internals.EntityConfigurations;

public class UfEntityConfiguration : IEntityTypeConfiguration<UfDto>
{
    public void Configure(
        EntityTypeBuilder<UfDto> builder
    )
    {
        builder.HasKey(key => key.CodigoUf);

        builder.Property(prop => prop.CodigoUf).HasColumnName("codigo_uf");
        builder.Property(prop => prop.UfShortName).HasColumnName("uf");
        builder.Property(prop => prop.Name).HasColumnName("nome");
        builder.Property(prop => prop.Latitude).HasColumnName("latitude");
        builder.Property(prop => prop.Longitude).HasColumnName("longitude");
        builder.Property(prop => prop.Regiao).HasColumnName("regiao");

        builder.HasMany(prop => prop.Cidades)
            .WithOne(prop => prop.Uf)
            .HasForeignKey(key => key.CodigoUf);

        builder.ToTable("Estados");
    }
}