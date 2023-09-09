namespace SqliteDatasource.Internals.Dto;

public sealed record UfDto(
    int CodigoUf
    , string UfShortName
    , string Name
    , double Latitude
    , double Longitude
    , string Regiao
)
{
    public IEnumerable<CidadeDto> Cidades
    {
        get;
        set;
    }
}