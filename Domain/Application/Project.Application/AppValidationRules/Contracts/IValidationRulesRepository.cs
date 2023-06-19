using Common.Optional;

using Core.Entities.ValidationRules;

namespace ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;
public interface IValidationRulesRepository
{
    public Task CreateValidationRule(ValidationRule validationRule);

    public Task<Optional<IEnumerable<ValidationRule>>> LoadValidationRule(string validationItemPath);
}
