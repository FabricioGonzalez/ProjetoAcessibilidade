using Core.Entities.ValidationRules;
using LanguageExt;

namespace ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;

public interface IValidationRulesRepository
{
    public Task CreateValidationRule(
        ValidationRule validationRule
    );

    public Task<Option<IEnumerable<ValidationRule>>> LoadValidationRule(
        string validationItemPath
    );
}