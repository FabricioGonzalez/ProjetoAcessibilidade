using SqliteDatasource.Internals.Dto;

namespace SqliteDatasource.Internals;

public sealed class LocationRepository
{
    public IEnumerable<UfDto> GetAllUf() => ItemDataContext
        .CreateContext()
        .Set<UfDto>()
        .OrderBy(it => it.Name)
        .ToList();

    public IEnumerable<CidadeDto> GetAllCitiesByUf(
        int codUf
    ) => ItemDataContext
        .CreateContext()
        .Set<CidadeDto>()
        .Where(it => it.CodigoUf == codUf)
        .OrderBy(it => it.Nome)
        .ToList();
}