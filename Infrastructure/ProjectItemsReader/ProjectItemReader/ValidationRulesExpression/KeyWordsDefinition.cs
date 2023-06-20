namespace ProjectItemReader.ValidationRulesExpression;
internal static class KeyWordsDefinition
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
}
