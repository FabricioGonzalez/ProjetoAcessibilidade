using System.Globalization;
using System.Text.RegularExpressions;
using XmlDatasource.ValidationRules.DTO;

namespace XmlDatasource.ValidationRules.ValidationRulesExpression;

public class RuleLexer
{
    public string MountExpressionFrom(
        RuleConditionItem appCondition
    ) =>
        /*KeyWordsDefinition.Operations.TryGetValue(key: appCondition.Type, value: out var op);

        return appCondition.Type switch
        {
            "has" => $"{{{appCondition.TargetId}}} {appCondition.Type} ${appCondition.CheckingValue}$"
            , _ => $"{{{appCondition.TargetId}}} {appCondition.Type} {appCondition.CheckingValue} "
        };*/
        "";

    public string GetTargetId(
        string instruction
    )
    {
        var id = Regex.Match(input: instruction
            , pattern:
            @$"\{KeyWordsDefinition.EvluationTargetStartKey}(.*)\{KeyWordsDefinition.EvluationTargetEndKey}");

        return id.Value[1..^1];
    }

    public string GetCheckingValue(
        string instruction
    )
    {
        var id = Regex.Match(input: instruction
            , pattern: @$"\{KeyWordsDefinition.EvluationValueKey}(.*?)\{KeyWordsDefinition.EvluationValueKey}");

        if (id.Value.Length > 0)
        {
            return id.Value[1..^1];
        }

        return "";
    }

    public IEnumerable<string> GetEvaluationExpression(
        string instruction
    )
    {
        var items = Regex
            .Split(
                input: Regex.Match(input: instruction
                        , pattern:
                        @$"(({KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationGreatValueExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression})|({KeyWordsDefinition.EvaluationMatchingExpression})|({KeyWordsDefinition.EvaluationBooleanExpression}))\s(.*)")
                    .Value
                , pattern:
                @$"(({KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationGreatValueExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression})|({KeyWordsDefinition.EvaluationMatchingExpression})|({KeyWordsDefinition.EvaluationBooleanExpression}))\s")
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return items;
    }

    public (string target, string checkingValue, IEnumerable<string> evaluation) GetEvaluation(
        string evaluatingExpression
    ) => (GetTargetId(evaluatingExpression), GetCheckingValue(evaluatingExpression)
        , GetEvaluationExpression(evaluatingExpression));

    public static bool Compare(
        string op
        , string left
        , string right
    ) =>
        op switch
        {
            "<" => double.Parse(s: left, provider: CultureInfo.InvariantCulture) <
                   double.Parse(s: right, provider: CultureInfo.InvariantCulture)
            , ">" => double.Parse(s: left, provider: CultureInfo.InvariantCulture) >
                     double.Parse(s: right, provider: CultureInfo.InvariantCulture)
            , "<=" => double.Parse(s: left, provider: CultureInfo.InvariantCulture) >=
                      double.Parse(s: right, provider: CultureInfo.InvariantCulture)
            , ">=" => double.Parse(s: left, provider: CultureInfo.InvariantCulture) <=
                      double.Parse(s: right, provider: CultureInfo.InvariantCulture)
            , "is" => left.Equals(right), "has" => left.Contains(right.Replace(oldValue: "$", newValue: "")), _ => false
        };

    public Func<bool> MountEvaluation(
        string checkingValue
        , string evaluationType
        , string targetValue
    ) => () => KeyWordsDefinition.Operations.TryGetValue(key: evaluationType, value: out var op) &&
               Compare(op: op, left: checkingValue, right: targetValue.Trim());
}