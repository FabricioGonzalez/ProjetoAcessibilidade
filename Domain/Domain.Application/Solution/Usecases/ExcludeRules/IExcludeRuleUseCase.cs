namespace Application.Solution.Usecases.ExcludeRules;

public interface IExcludeRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}