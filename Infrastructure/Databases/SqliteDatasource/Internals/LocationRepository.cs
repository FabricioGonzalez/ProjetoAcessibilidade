using SqliteDatasource.Internals.Dto;

namespace SqliteDatasource.Internals;

public sealed class LocationRepository
{
    public IEnumerable<UfDto> GetAllUf() => ItemDataContext.CreateContext().Set<UfDto>().ToList();

    public IEnumerable<CidadeDto> GetAllCitiesByUf(
        int codUf
    ) => ItemDataContext.CreateContext().Set<CidadeDto>().Where(it => it.CodigoUf == codUf).ToList();
}