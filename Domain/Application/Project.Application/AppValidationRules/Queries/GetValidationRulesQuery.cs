using Common;

using Core.Entities.ValidationRules;

using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;
using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjetoAcessibilidade.Domain.AppValidationRules.Queries;
public sealed record GetValidationRulesQuery(string ItemPath) : IRequest<Resource<IEnumerable<ValidationRule>>>;


public sealed class GetValidationRulesQueryHandler : IHandler<GetValidationRulesQuery, Resource<IEnumerable<ValidationRule>>>
{
    private readonly IValidationRulesRepository _repository;
    public GetValidationRulesQueryHandler(IValidationRulesRepository repository)
    {
        _repository = repository;
    }
    public async Task<Resource<IEnumerable<ValidationRule>>> HandleAsync(
        GetValidationRulesQuery command,
        CancellationToken cancellation
    )
    {
        var result = await _repository.LoadValidationRule(command.ItemPath);

        return result
            .Map<Resource<IEnumerable<ValidationRule>>>(item => new Resource<IEnumerable<ValidationRule>>.Success(item))
            .Reduce(() => new Resource<IEnumerable<ValidationRule>>.Error("Nenhuma Regra foi encontrada", Enumerable.Empty<ValidationRule>()));
    }
}
