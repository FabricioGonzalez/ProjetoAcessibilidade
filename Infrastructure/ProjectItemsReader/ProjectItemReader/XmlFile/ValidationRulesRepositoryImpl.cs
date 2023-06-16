using System.Xml.Serialization;

using Common.Optional;

using Core.Entities.ValidationRules;

using ProjectItemReader.XmlFile.ValidationRules;

using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;

namespace ProjectItemReader.XmlFile;
public class ValidationRulesRepositoryImpl : IValidationRulesRepository
{
    public async Task CreateValidationRule(ValidationRule validationRule)
    {

    }
    public async Task<Optional<ValidationRule>> LoadValidationRule(string validationItemPath)
    {
        return Optional<ValidationRule>.None();
    }

    public XmlSerializer CreateSerealizer() => new(type: typeof(ValidationItemRoot));
}
