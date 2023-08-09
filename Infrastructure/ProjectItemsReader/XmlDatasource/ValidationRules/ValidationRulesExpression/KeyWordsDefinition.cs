namespace XmlDatasource.ValidationRulesExpression;
public static class KeyWordsDefinition
{
    public const char EvluationTargetStartKey = '{';
    public const char EvluationTargetEndKey = '}';

    public const char EvluationValueKey = '#';

    public const char EvluationTargetStringStateKey = '$';

    public const string EvaluationBooleanExpression = "is";
    public const string EvaluationBooleanValue = "checked";
    public const string EvaluationBooleanNotValue = "unchecked";
    public const string EvaluationGreatValueExpression = "more";
    public const string EvaluationMinorValueExpression = "less";
    public const string EvaluationQuantityEqualityExpression = "than";
    public const string EvaluationMatchingExpression = "has";

    public static Dictionary<string, string> Operations = new()
    {
        {EvaluationBooleanExpression,"is" },
        {EvaluationGreatValueExpression,">" },
        {EvaluationMinorValueExpression,"<" },
        {EvaluationMatchingExpression,"has" },
        {$"{EvaluationGreatValueExpression} {EvaluationQuantityEqualityExpression}" ,">=" },
        {$"{EvaluationMinorValueExpression} {EvaluationQuantityEqualityExpression}" ,"<=" },
    };

}

