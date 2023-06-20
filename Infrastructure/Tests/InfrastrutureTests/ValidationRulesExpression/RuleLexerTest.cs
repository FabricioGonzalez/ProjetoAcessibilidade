using ProjectItemReader.ValidationRulesExpression;

namespace InfrastrutureTests.ValidationRulesExpression;

public class RuleLexerTest
{
    private readonly RuleLexer lexer;
    public string _evaluation;
    public string _evaluation2;
    public string _evaluation3;
    public string _evaluation4;
    public string _evaluation5;
    public RuleLexerTest()
    {
        lexer = new RuleLexer();
        _evaluation = "{TargetId} -> #True# is checked";
        _evaluation2 = "{TargetId} -> #1.1# more 1";
        _evaluation3 = "{TargetId} -> #0.4# less 1";
        _evaluation4 = "{TargetId} -> #1# more than 1";
        _evaluation5 = "{TargetId} -> #1# less than 1";
    }

    [Fact()]
    public void GetTargetIdTest()
    {
        var result = lexer.GetTargetId(_evaluation);
        var result2 = lexer.GetTargetId(_evaluation2);
        var result3 = lexer.GetTargetId(_evaluation3);
        var result4 = lexer.GetTargetId(_evaluation4);
        var result5 = lexer.GetTargetId(_evaluation5);

        Assert.True(!string.IsNullOrWhiteSpace(result), $"O resultado é invalido {result}");
        Assert.True(!string.IsNullOrWhiteSpace(result2), $"O resultado é invalido {result2}");
        Assert.True(!string.IsNullOrWhiteSpace(result3), $"O resultado é invalido {result3}");
        Assert.True(!string.IsNullOrWhiteSpace(result4), $"O resultado é invalido {result4}");
        Assert.True(!string.IsNullOrWhiteSpace(result5), $"O resultado é invalido {result5}");
    }

    [Fact()]
    public void GetCheckingValueTest()
    {
        var result = lexer.GetCheckingValue(_evaluation);
        var result2 = lexer.GetCheckingValue(_evaluation2);
        var result3 = lexer.GetCheckingValue(_evaluation3);
        var result4 = lexer.GetCheckingValue(_evaluation4);
        var result5 = lexer.GetCheckingValue(_evaluation5);

        Assert.True(!string.IsNullOrWhiteSpace(result), $"O resultado é invalido {result}");
        Assert.True(!string.IsNullOrWhiteSpace(result2), $"O resultado é invalido {result2}");
        Assert.True(!string.IsNullOrWhiteSpace(result3), $"O resultado é invalido {result3}");
        Assert.True(!string.IsNullOrWhiteSpace(result4), $"O resultado é invalido {result4}");
        Assert.True(!string.IsNullOrWhiteSpace(result5), $"O resultado é invalido {result5}");
    }
    [Fact()]
    public void GetEvaluationExpressionTest()
    {
        var result = lexer.GetEvaluationExpression(_evaluation);
        var result2 = lexer.GetEvaluationExpression(_evaluation2);
        var result3 = lexer.GetEvaluationExpression(_evaluation3);
        var result4 = lexer.GetEvaluationExpression(_evaluation4);
        var result5 = lexer.GetEvaluationExpression(_evaluation5);

        Assert.True(result.Count() > 0, $"O resultado é invalido {result}");
        Assert.True(result2.Count() > 0, $"O resultado é invalido {result2}");
        Assert.True(result3.Count() > 0, $"O resultado é invalido {result3}");
        Assert.True(result4.Count() > 0, $"O resultado é invalido {result4}");
        Assert.True(result5.Count() > 0, $"O resultado é invalido {result5}");
    }
    [Fact()]
    public void MountCompareExpression()
    {
        var result = GetEvaluation(_evaluation);
        var result2 = GetEvaluation(_evaluation2);
        var result3 = GetEvaluation(_evaluation3);
        var result4 = GetEvaluation(_evaluation4);
        var result5 = GetEvaluation(_evaluation5);

        var testResult = lexer.MountEvaluation(result.checkingValue, result.evaluation.First(), result.evaluation.Last())();
        var testResult2 = lexer.MountEvaluation(result2.checkingValue, result2.evaluation.First(), result2.evaluation.Last())();
        var testResult3 = lexer.MountEvaluation(result3.checkingValue, result3.evaluation.First(), result3.evaluation.Last())();
        var testResult4 = lexer.MountEvaluation(result4.checkingValue, result4.evaluation.First(), result4.evaluation.Last())();
        var testResult5 = lexer.MountEvaluation(result5.checkingValue, result5.evaluation.First(), result5.evaluation.Last())();

        Assert.True(testResult, "Erro ao Validar a função");
        Assert.True(testResult2, "Erro ao Validar a função");
        Assert.True(testResult3, "Erro ao Validar a função");
        Assert.True(testResult4, "Erro ao Validar a função");
        Assert.True(testResult5, "Erro ao Validar a função");

    }

    private (string target, string checkingValue, IEnumerable<string> evaluation) GetEvaluation(string evaluatingExpression)
    {
        return (lexer.GetTargetId(evaluatingExpression), lexer.GetCheckingValue(evaluatingExpression), lexer.GetEvaluationExpression(evaluatingExpression));
    }
}