using System.Collections.Generic;
using System.Linq;
using ProjectAvalonia.Common.Models.App;
using SqliteDatasource.Internals;

namespace ProjectAvalonia.Common.Services.LocationService;

public class LocationService
{
    private readonly LocationRepository _repository;

    public LocationService()
    {
        _repository = new LocationRepository();
    }

    public IEnumerable<Uf> GetAllUfs() => _repository.GetAllUf()
        .Select(it => new Uf(Code: it.CodigoUf, ShortName: it.UfShortName, Name: it.Name));

    public IEnumerable<Cidade> GetCidades(
        int codigoUf
    ) =>
        _repository.GetAllCitiesByUf(codigoUf)
            .Select(it => new Cidade(CodigoUf: it.CodigoUf, Nome: it.Nome));
}