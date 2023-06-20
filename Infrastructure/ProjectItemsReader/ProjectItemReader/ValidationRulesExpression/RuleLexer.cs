using System.Text.RegularExpressions;

namespace ProjectItemReader.ValidationRulesExpression;

public class RuleLexer
{
    public string GetTargetId(string instruction)
    {
        var id = Regex.Match(instruction, @$"\{KeyWordsDefinition.EvluationTargetStartKey}(.*?)\{KeyWordsDefinition.EvluationTargetEndKey}");

        return id.Value[1..^1];
    }

    public string GetCheckingValue(string instruction)
    {
        var id = Regex.Match(instruction, @$"\{KeyWordsDefinition.EvluationValueKey}(.*?)\{KeyWordsDefinition.EvluationValueKey}");

        return id.Value[1..^1];
    }

    public IEnumerable<string> GetEvaluationExpression(string instruction)
    {
        var items = Regex.Split(Regex.Match(instruction, @$"(({KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationGreatValueExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression})|({KeyWordsDefinition.EvaluationMatchingExpression})|({KeyWordsDefinition.EvaluationBooleanExpression}))\s(.*)").Value, @$"(({KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression})|({KeyWordsDefinition.EvaluationGreatValueExpression})|({KeyWordsDefinition.EvaluationMinorValueExpression})|({KeyWordsDefinition.EvaluationMatchingExpression})|({KeyWordsDefinition.EvaluationBooleanExpression}))\s")
            .Where(x => !string.IsNullOrWhiteSpace(x));

        return items;
    }

    public Func<bool> MountEvaluation(string checkingValue, string evaluationType, string targetValue)
    {
        if (evaluationType == KeyWordsDefinition.EvaluationBooleanExpression)
        {
            return () => checkingValue == "True" && targetValue.Trim() == "checked" ? true : checkingValue == "False" && targetValue == "unchecked" ? true : false;
        }
        if (evaluationType == KeyWordsDefinition.EvaluationMatchingExpression)
        {
            return () => checkingValue.Contains(targetValue);
        }
        if (evaluationType == KeyWordsDefinition.EvaluationGreatValueExpression)
        {
            return () => double.Parse(checkingValue) > double.Parse(targetValue.Trim());
        }
        if (evaluationType == KeyWordsDefinition.EvaluationMinorValueExpression)
        {
            return () => double.Parse(checkingValue) < double.Parse(targetValue.Trim());
        }
        if (evaluationType == $"{KeyWordsDefinition.EvaluationGreatValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression}")
        {
            return () => double.Parse(checkingValue) >= double.Parse(targetValue.Trim());
        }
        if (evaluationType == $"{KeyWordsDefinition.EvaluationMinorValueExpression} {KeyWordsDefinition.EvaluationQuantityEqualityExpression}")
        {
            return () => double.Parse(checkingValue) <= double.Parse(targetValue.Trim());
        }
        return () => false;

    }
}

public class ExpressionObject
{

}

public class ExpressionConverter
{

}