using System.Xml.Serialization;

using Common;
using Common.Optional;

using Core.Entities.ValidationRules;

using ProjectItemReader.ValidationRulesExpression;
using ProjectItemReader.XmlFile.ValidationRules;

using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;

namespace ProjectItemReader.XmlFile;
public class ValidationRulesRepositoryImpl : IValidationRulesRepository
{
    private RuleLexer _ruleLexer;

    public ValidationRulesRepositoryImpl(RuleLexer ruleLexer)
    {
        _ruleLexer = ruleLexer;
    }

    public async Task CreateValidationRule(ValidationRule validationRule)
    {
    }
    public async Task<Optional<IEnumerable<ValidationRule>>> LoadValidationRule(string validationItemPath)
    {
        return validationItemPath
            .ToOption()
            .Map(item =>
            CreateSerealizer()
            .Map<Resource<ValidationItemRoot>>(xmlReader =>
            {
                if (File.Exists(item))
                {
                    using var reader = new StreamReader(item);

                    var result = (ValidationItemRoot)xmlReader.Deserialize(reader);

                    return new Resource<ValidationItemRoot>.Success(result);
                }
                return new Resource<ValidationItemRoot>.Error("Arquivo não Existe", default);
            })
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
                    Rules = x.RuleConditions.Select(x =>
                    {

                        var conditions = x.RuleSetItems.Select(x =>
                        {
                            var res = _ruleLexer.GetEvaluation(x.ValueTrigger);

                            return new Conditions(
                                TargetId: res.target,
                                Type: res.evaluation.First(),
                                CheckingValue: res.evaluation.Last(),
                                Result: x.Results.Select(result => result.Result),
                                ConditionsFunctions: new((item) =>
                            {
                                var expression = _ruleLexer.MountEvaluation(item, res.evaluation.First(), res.evaluation.Last());

                                return (expression(), x.Results.Select(x => x.Result));
                            }));
                        });

                        var results = x.RuleSetItems.SelectMany(x => x.Results.Select(y => y.Result));

                        return new RuleSet()
                        {
                            Operation = x.Operation,
                            Conditions = conditions
                        };
                    })
                });
            })
            .Reduce(() => Enumerable.Empty<ValidationRule>()));

    }

    public Optional<XmlSerializer> CreateSerealizer() => Optional<XmlSerializer>.Some(new(type: typeof(ValidationItemRoot)));



}
