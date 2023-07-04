using Avalonia;
using Avalonia.Data.Core;
using Common.Models;
using Core.Entities.ValidationRules;
using LanguageExt.Common;
using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;
using ProjetoAcessibilidade.Domain.Contracts;

namespace ProjetoAcessibilidade.Domain.AppValidationRules.Commands;

public sealed record SaveValidationRulesCommand(IEnumerable<ValidationRule> Rules, string validationRulesPath) : IRequest<Result<Empty>>;


public sealed class SaveValidationRulesCommandHandler : IHandler<SaveValidationRulesCommand, Result<Empty>>
{

    private IValidationRulesRepository _rulesRepository;

    public SaveValidationRulesCommandHandler(
        IValidationRulesRepository rulesRepository
    )
    {
        _rulesRepository = rulesRepository;
    }

    public async Task<Result<Empty>> HandleAsync(
        SaveValidationRulesCommand query
        , CancellationToken cancellation
    )
    {
        await _rulesRepository.CreateValidationRule(query.Rules,query.validationRulesPath);

        return new Result<Empty>(Empty.Value);
    }
}