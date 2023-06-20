using System.Xml;
using System.Xml.Serialization;

using Common;
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
    public async Task<Optional<IEnumerable<ValidationRule>>> LoadValidationRule(string validationItemPath)
    {
        return validationItemPath
            .ToOption()
            .Map(item =>
            CreateSerealizer()
            .Map(xmlReader =>
            {
                using var reader = XmlReader.Create(inputUri: item);

                return (ValidationItemRoot)xmlReader.Deserialize(reader);
            })
            .Map<Resource<ValidationItemRoot>>(item => new Resource<ValidationItemRoot>.Success(item))
            .Reduce(() => new Resource<ValidationItemRoot>.Error("Erro ao deserializar arquivo", default)))
            .Map(item =>
            {
                if (item is Resource<ValidationItemRoot>.Error)
                {
                    return Enumerable.Empty<ValidationRule>();
                }

                return ((Resource<ValidationItemRoot>.Success)item)
                .Data
                .Rules
                .Select(x => new ValidationRule()
                {
                    Targets = x.Targets.Select(x => new Targets() { Id = x.Id }),
                    Rules = x.RuleConditions.Select(x => new RuleSet()
                    {
                        Operation = x.Operation,
                        /* Conditions = x.RuleSetItems.Select()*/
                    })
                });
            });
    }

    public Optional<XmlSerializer> CreateSerealizer() => Optional<XmlSerializer>.Some(new(type: typeof(ValidationItemRoot)));



}
