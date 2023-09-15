using System.Collections.Generic;
using System.Linq;

using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.Models;

using SqliteDatasource.Internals;

namespace ProjectAvalonia.Common.Services.LocationService;

public class LocationService : ILocationService
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
    public Uf GetUfByName(string uf) => _repository.GetAllUf()
        .Select(it => new Uf(Code: it.CodigoUf, ShortName: it.UfShortName, Name: it.Name))
        .FirstOrDefault(it => it.ShortName == uf);
}