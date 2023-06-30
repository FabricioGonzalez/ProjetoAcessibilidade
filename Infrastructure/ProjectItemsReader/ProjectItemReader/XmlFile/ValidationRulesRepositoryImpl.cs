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
    private readonly RuleLexer _ruleLexer;
    private Optional<XmlSerializer> _serializer;

    public ValidationRulesRepositoryImpl(RuleLexer ruleLexer)
    {
        _ruleLexer = ruleLexer;
    }

    public async Task CreateValidationRule(ValidationRule validationRule)
    {
    }

    public async Task<Optional<IEnumerable<ValidationRule>>> LoadValidationRule(string validationItemPath) =>
        validationItemPath
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

                        return new Resource<ValidationItemRoot>.Error("Arquivo não Existe");
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
                            .Select(x => new ValidationRule
                            {
                                Target = new Target { Id = x.Target.Id },

                                Rules = x.RuleConditions.Select(x =>
                                {
                                    var conditions = x.RuleSetItems.Select(x =>
                                    {
                                        var res = _ruleLexer.GetEvaluation(x.ValueTrigger);

                                        return new Conditions(
                                            res.target,
                                            res.evaluation.First(),
                                            res.evaluation.Last(),
                                            x.Results.Select(result => result.Result),
                                            item =>
                                            {
                                                var expression = _ruleLexer.MountEvaluation(item,
                                                    res.evaluation.First(), res.evaluation.Last());

                                                return (expression(), x.Results.Select(x => x.Result));
                                            });
                                    });

                                    var results = x.RuleSetItems.SelectMany(x => x.Results.Select(y => y.Result));

                                    return new RuleSet
                                    {
                                        Operation = x.Operation,
                                        Conditions = conditions
                                    };
                                })
                            });
                    })
                    .Reduce(() => Enumerable.Empty<ValidationRule>()));

    public Optional<XmlSerializer> CreateSerealizer() => _serializer.IsNone
        ? _serializer = Optional<XmlSerializer>.Some(new XmlSerializer(typeof(ValidationItemRoot)))
        : _serializer;
}