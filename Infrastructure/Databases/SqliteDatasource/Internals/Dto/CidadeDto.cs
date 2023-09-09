namespace SqliteDatasource.Internals.Dto;

public record CidadeDto(
    int CodigoIbge
    , string Nome
    , double Latitude
    , double Longitude
    , int Capital
    , int CodigoUf
    , int SiafiId
    , int Ddd
    , string FusoHorario
)
{
    public UfDto Uf
    {
        get;
        set;
    }
}