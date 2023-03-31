﻿using Core.Entities.App;

using Project.Domain.Contracts;

namespace Project.Domain.App.Queries.GetUFList;

public sealed record GetAllUFQuery : IRequest<IList<UFModel>>
{
}

public sealed class GetAllUFQueryHandler : IQueryHandler<GetAllUFQuery, IList<UFModel>>
{
    public async Task<IList<UFModel>> Handle(GetAllUFQuery query, CancellationToken cancellation)
    {
        return new List<UFModel>()
        {
new UFModel("Rondônia","RO"),
new UFModel("Acre","AC"),
new UFModel("Amazonas","AM"),
new UFModel("Roraima","RR"),
new UFModel("Pará","PA"),
new UFModel("Amapá","AP"),
new UFModel("Tocantins","TO"),
new UFModel("Maranhão","MA"),
new UFModel("Piauí","PI"),
new UFModel("Ceará","CE"),
new UFModel("Rio Grande do Norte","RN"),
new UFModel("Paraíba","PB"),
new UFModel("Pernambuco","PE"),
new UFModel("Alagoas","AL"),
new UFModel("Sergipe","SE"),
new UFModel("Bahia","BA"),
new UFModel("Minas Gerais","MG"),
new UFModel("Espírito Santo","ES"),
new UFModel("Rio de Janeiro","RJ"),
new UFModel("São Paulo","SP"),
new UFModel("Paraná","PR"),
new UFModel("Santa Catarina","SC"),
new UFModel("Rio Grande do Sul","RS"),
new UFModel("Mato Grosso do Sul","MS"),
new UFModel("Mato Grosso","MT"),
new UFModel("Goiás","GO"),
new UFModel("Distrito Federal","DF")
        };
    }
}
