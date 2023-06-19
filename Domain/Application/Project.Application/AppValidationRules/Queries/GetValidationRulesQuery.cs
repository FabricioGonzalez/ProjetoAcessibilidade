using Common;

using Core.Entities.ValidationRules;

using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;
using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjetoAcessibilidade.Domain.AppValidationRules.Queries;
public sealed record GetValidationRulesQuery(string ItemPath) : IRequest<Resource<ValidationRule>>;


public sealed class GetValidationRulesQueryHandler : IHandler<GetValidationRulesQuery, Resource<ValidationRule>>
{
    private readonly IValidationRulesRepository _repository;
    public GetValidationRulesQueryHandler(IValidationRulesRepository repository)
    {
        _repository = repository;
    }
    public async Task<Resource<ValidationRule>> HandleAsync(
        GetValidationRulesQuery command,
        CancellationToken cancellation
    )
    {
        var result = await _repository.LoadValidationRule(command.ItemPath);

        return new Resource<ValidationRule>.Success(default);

        /* return result.Map(item =>
         {
             return new Resource<ValidationRule>.Success(item)
         });*/
    }
}
