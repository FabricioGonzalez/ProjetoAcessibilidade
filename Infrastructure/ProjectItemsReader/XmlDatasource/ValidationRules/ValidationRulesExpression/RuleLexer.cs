using System.Globalization;
using System.Text.RegularExpressions;
using Core.Entities.ValidationRules;

namespace XmlDatasource.ValidationRulesExpression;

public class RuleLexer
{
    public string MountExpressionFrom(
        Conditions appCondition
    )
    {
        KeyWordsDefinition.Operations.TryGetValue(appCondition.Type, out var op);

        return appCondition.Type switch
        {
            "has" => $"{{{appCondition.TargetId}}} {appCondition.Type} ${appCondition.CheckingValue}$"
            , _ => $"{{{appCondition.TargetId}}} {appCondition.Type} {appCondition.CheckingValue} "
        };
    }

    public string GetTargetId(
        string instruction
    )
    {
        var id = Regex.Match(instruction
            , @$"\{KeyWordsDefinition.EvluationTargetStartKey}(.*)\{KeyWordsDefinition.EvluationTargetEndKey}");

        return id.Value[1..^1];
    }

    public string GetCheckingValue(
        string instruction
    )
    {
        var id = Regex.Match(instruction
            , @$"\{KeyWordsDefinition.EvluationValueKey}(.*?)\{KeyWordsDefinition.EvluationValueKey}");

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
                Regex.Match(instruction
                        , @$"(({KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationGreatValueExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression})|({KeyWordsDefinition.EvaluationMatchingExpression})|({KeyWordsDefinition.EvaluationBooleanExpression}))\s(.*)")
                    .Value
                , @$"(({KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationGreatValueExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression})|({KeyWordsDefinition.EvaluationMatchingExpression})|({KeyWordsDefinition.EvaluationBooleanExpression}))\s")
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
            "<" => double.Parse(left, CultureInfo.InvariantCulture) < double.Parse(right, CultureInfo.InvariantCulture)
            , ">" => double.Parse(left, CultureInfo.InvariantCulture) >
                     double.Parse(right, CultureInfo.InvariantCulture)
            , "<=" => double.Parse(left, CultureInfo.InvariantCulture) >=
                      double.Parse(right, CultureInfo.InvariantCulture)
            , ">=" => double.Parse(left, CultureInfo.InvariantCulture) <=
                      double.Parse(right, CultureInfo.InvariantCulture)
            , "is" => left.Equals(right), "has" => left.Contains(right.Replace("$", "")), _ => false
        };

    public Func<bool> MountEvaluation(
        string checkingValue
        , string evaluationType
        , string targetValue
    ) => () => KeyWordsDefinition.Operations.TryGetValue(evaluationType, out var op) &&
               Compare(op, checkingValue, targetValue.Trim());
}