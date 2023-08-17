using ProjectAvalonia.Presentation.Models;

namespace ProjectAvalonia.Presentation.Interfaces.Services;

public interface ILocationService
{
    public IEnumerable<Uf> GetAllUfs();

    public IEnumerable<Cidade> GetCidades(
        int codigoUf
    );
}