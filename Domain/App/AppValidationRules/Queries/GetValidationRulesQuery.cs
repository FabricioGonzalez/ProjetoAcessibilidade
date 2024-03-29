﻿using Core.Entities.ValidationRules;
using LanguageExt.Common;
using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;
using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjetoAcessibilidade.Domain.AppValidationRules.Queries;

public sealed record GetValidationRulesQuery(
    string ItemPath
) : IRequest<Result<IEnumerable<ValidationRule>>>;

public sealed class
    GetValidationRulesQueryHandler : IHandler<GetValidationRulesQuery, Result<IEnumerable<ValidationRule>>>
{
    private readonly IValidationRulesRepository _repository;

    public GetValidationRulesQueryHandler(
        IValidationRulesRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ValidationRule>>> HandleAsync(
        GetValidationRulesQuery command
        , CancellationToken cancellation
    )
    {
        var result = await _repository.LoadValidationRule(command.ItemPath);

        return result
            .Match(item => new Result<IEnumerable<ValidationRule>>(item),
                () => new Result<IEnumerable<ValidationRule>>(new Exception("Nenhuma Regra foi encontrada")));
    }
}