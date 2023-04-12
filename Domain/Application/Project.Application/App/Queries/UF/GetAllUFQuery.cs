using Core.Entities.App;
using Project.Domain.Contracts;

namespace Project.Domain.App.Queries.UF;

public sealed record GetAllUfQuery : IRequest<IList<UFModel>>;

public sealed class GetAllUfQueryHandler : IQueryHandler<GetAllUfQuery, IList<UFModel>>
{
    public async Task<IList<UFModel>> Handle(
        GetAllUfQuery query
        , CancellationToken cancellation
    ) =>
        new List<UFModel>
        {
            new(code: "Rondônia", name: "RO"), new(code: "Acre", name: "AC"), new(code: "Amazonas", name: "AM")
            , new(code: "Roraima", name: "RR"), new(code: "Pará", name: "PA")
            , new(code: "Amapá", name: "AP"), new(code: "Tocantins", name: "TO"), new(code: "Maranhão", name: "MA")
            , new(code: "Piauí", name: "PI"), new(code: "Ceará", name: "CE")
            , new(code: "Rio Grande do Norte", name: "RN"), new(code: "Paraíba", name: "PB")
            , new(code: "Pernambuco", name: "PE"), new(code: "Alagoas", name: "AL")
            , new(code: "Sergipe", name: "SE"), new(code: "Bahia", name: "BA"), new(code: "Minas Gerais", name: "MG")
            , new(code: "Espírito Santo", name: "ES")
            , new(code: "Rio de Janeiro", name: "RJ"), new(code: "São Paulo", name: "SP")
            , new(code: "Paraná", name: "PR"), new(code: "Santa Catarina", name: "SC")
            , new(code: "Rio Grande do Sul", name: "RS"), new(code: "Mato Grosso do Sul", name: "MS")
            , new(code: "Mato Grosso", name: "MT")
            , new(code: "Goiás", name: "GO"), new(code: "Distrito Federal", name: "DF")
        };
}