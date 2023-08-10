using XmlDatasource.ValidationRules.ValidationRulesExpression;

namespace InfrastrutureTests.ValidationRulesExpression;

public class RuleLexerTest
{
    private readonly RuleLexer lexer;
    public string _evaluation;
    public string _evaluation2;
    public string _evaluation3;
    public string _evaluation4;
    public string _evaluation5;
    public string _evaluation6;

    public RuleLexerTest()
    {
        lexer = new RuleLexer();
        _evaluation = "{TargetId} -> #checked# is checked";
        _evaluation2 = "{TargetId} -> #Teste# has $Teste$";
        _evaluation3 = "{TargetId} -> #1.1# more 1";
        _evaluation4 = "{TargetId} -> #0.4# less 1";
        _evaluation5 = "{TargetId} -> #1# more than 1";
        _evaluation6 = "{TargetId} -> #1# less than 1";
    }

    [Fact]
    public void GetTargetIdTest()
    {
        var result = lexer.GetTargetId(_evaluation);
        var result2 = lexer.GetTargetId(_evaluation2);
        var result3 = lexer.GetTargetId(_evaluation3);
        var result4 = lexer.GetTargetId(_evaluation4);
        var result5 = lexer.GetTargetId(_evaluation5);
        var result6 = lexer.GetTargetId(_evaluation6);

        Assert.True(condition: !string.IsNullOrWhiteSpace(result), userMessage: $"O resultado é invalido {result}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result2), userMessage: $"O resultado é invalido {result2}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result3), userMessage: $"O resultado é invalido {result3}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result4), userMessage: $"O resultado é invalido {result4}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result5), userMessage: $"O resultado é invalido {result5}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result6), userMessage: $"O resultado é invalido {result6}");
    }

    [Fact]
    public void GetCheckingValueTest()
    {
        var result = lexer.GetCheckingValue(_evaluation);
        var result2 = lexer.GetCheckingValue(_evaluation2);
        var result3 = lexer.GetCheckingValue(_evaluation3);
        var result4 = lexer.GetCheckingValue(_evaluation4);
        var result5 = lexer.GetCheckingValue(_evaluation5);
        var result6 = lexer.GetCheckingValue(_evaluation6);

        Assert.True(condition: !string.IsNullOrWhiteSpace(result), userMessage: $"O resultado é invalido {result}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result2), userMessage: $"O resultado é invalido {result2}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result3), userMessage: $"O resultado é invalido {result3}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result4), userMessage: $"O resultado é invalido {result4}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result5), userMessage: $"O resultado é invalido {result5}");
        Assert.True(condition: !string.IsNullOrWhiteSpace(result6), userMessage: $"O resultado é invalido {result6}");
    }

    [Fact]
    public void GetEvaluationExpressionTest()
    {
        var result = lexer.GetEvaluationExpression(_evaluation);
        var result2 = lexer.GetEvaluationExpression(_evaluation2);
        var result3 = lexer.GetEvaluationExpression(_evaluation3);
        var result4 = lexer.GetEvaluationExpression(_evaluation4);
        var result5 = lexer.GetEvaluationExpression(_evaluation5);
        var result6 = lexer.GetEvaluationExpression(_evaluation6);

        Assert.True(condition: result.Count() > 0, userMessage: $"O resultado é invalido {result}");
        Assert.True(condition: result2.Count() > 0, userMessage: $"O resultado é invalido {result2}");
        Assert.True(condition: result3.Count() > 0, userMessage: $"O resultado é invalido {result3}");
        Assert.True(condition: result4.Count() > 0, userMessage: $"O resultado é invalido {result4}");
        Assert.True(condition: result5.Count() > 0, userMessage: $"O resultado é invalido {result5}");
        Assert.True(condition: result6.Count() > 0, userMessage: $"O resultado é invalido {result6}");
    }

    [Fact]
    public void MountCompareExpression()
    {
        var result = lexer.GetEvaluation(_evaluation);
        var result2 = lexer.GetEvaluation(_evaluation2);
        var result3 = lexer.GetEvaluation(_evaluation3);
        var result4 = lexer.GetEvaluation(_evaluation4);
        var result5 = lexer.GetEvaluation(_evaluation5);
        var result6 = lexer.GetEvaluation(_evaluation6);

        var testResult = lexer.MountEvaluation(checkingValue: result.checkingValue
            , evaluationType: result.evaluation.First(), targetValue: result.evaluation.Last())();
        var testResult2 = lexer.MountEvaluation(checkingValue: result2.checkingValue
            , evaluationType: result2.evaluation.First(), targetValue: result2.evaluation.Last())();
        var testResult3 = lexer.MountEvaluation(checkingValue: result3.checkingValue
            , evaluationType: result3.evaluation.First(), targetValue: result3.evaluation.Last())();
        var testResult4 = lexer.MountEvaluation(checkingValue: result4.checkingValue
            , evaluationType: result4.evaluation.First(), targetValue: result4.evaluation.Last())();
        var testResult5 = lexer.MountEvaluation(checkingValue: result5.checkingValue
            , evaluationType: result5.evaluation.First(), targetValue: result5.evaluation.Last())();
        var testResult6 = lexer.MountEvaluation(checkingValue: result6.checkingValue
            , evaluationType: result6.evaluation.First(), targetValue: result6.evaluation.Last())();

        Assert.True(condition: testResult, userMessage: "Erro ao Validar a função");
        Assert.True(condition: testResult2, userMessage: "Erro ao Validar a função");
        Assert.True(condition: testResult3, userMessage: "Erro ao Validar a função");
        Assert.True(condition: testResult4, userMessage: "Erro ao Validar a função");
        Assert.True(condition: testResult5, userMessage: "Erro ao Validar a função");
        Assert.True(condition: testResult6, userMessage: "Erro ao Validar a função");
    }
}