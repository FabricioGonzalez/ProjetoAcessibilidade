namespace Application.Usecases.FormRules.ExcludeRules;

public interface IExcludeRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}