using System.Xml.Serialization;
using Common;
using Core.Entities.ValidationRules;
using LanguageExt;
using LanguageExt.Common;
using ProjectItemReader.ValidationRulesExpression;
using ProjectItemReader.XmlFile.ValidationRules;
using ProjetoAcessibilidade.Domain.AppValidationRules.Contracts;

namespace ProjectItemReader.XmlFile;

public class ValidationRulesRepositoryImpl : IValidationRulesRepository
{
    private readonly RuleLexer _ruleLexer;
    private Option<XmlSerializer> _serializer;

    public ValidationRulesRepositoryImpl(
        RuleLexer ruleLexer
    )
    {
        _ruleLexer = ruleLexer;
    }

    public async Task CreateValidationRule(
        ValidationRule validationRule
    )
    {
    }

    public async Task<Option<IEnumerable<ValidationRule>>> LoadValidationRule(
        string validationItemPath
    ) =>
        Option<string>.Some(validationItemPath)
            .Map(item =>
                CreateSerealizer()
                    .Map<Result<ValidationItemRoot>>(xmlReader =>
                    {
                        try
                        {
                            if (File.Exists(item))
                            {
                                using var reader = new StreamReader(item);

                                var result = (ValidationItemRoot)xmlReader.Deserialize(reader);

                                return new Result<ValidationItemRoot>(result);
                            }

                            return new Result<ValidationItemRoot>(new Exception("Arquivo não Existe"));
                        }
                        catch (Exception e)
                        {
                            return new Result<ValidationItemRoot>(e);
                        }
                    })
                    .Map(item =>
                    {
                        if (item is Resource<ValidationItemRoot>.Error)
                        {
                            return Enumerable.Empty<ValidationRule>();
                        }

                        return item.Match(success => success.Rules.Select(x => new ValidationRule
                            {
                                Target = new Target { Id = x.Target.Id }, Rules = x.RuleConditions.Select(x =>
                                {
                                    var conditions = x.RuleSetItems.Select(x =>
                                    {
                                        var res = _ruleLexer.GetEvaluation(x.ValueTrigger);

                                        var type = res.evaluation.FirstOrDefault("is");
                                        var value = res.evaluation.LastOrDefault("");
                                        return new Conditions(
                                            res.target,
                                            type,
                                            value,
                                            x.Results.Select(result => result.Result),
                                            item =>
                                            {
                                                var expression = _ruleLexer.MountEvaluation(item,
                                                    type,
                                                    value);

                                                return (expression(), x.Results.Select(x => x.Result));
                                            });
                                    });

                                    var results = x.RuleSetItems.SelectMany(x => x.Results.Select(y => y.Result));

                                    return new RuleSet
                                    {
                                        Operation = x.Operation, Conditions = conditions
                                    };
                                })
                            }),
                            exception => Enumerable.Empty<ValidationRule>());
                    })
                    .Match(res => res, Enumerable.Empty<ValidationRule>));


    public Option<XmlSerializer> CreateSerealizer() =>
        _serializer.IsNone
            ? _serializer = Option<XmlSerializer>.Some(new XmlSerializer(typeof(ValidationItemRoot)))
            : _serializer;
}