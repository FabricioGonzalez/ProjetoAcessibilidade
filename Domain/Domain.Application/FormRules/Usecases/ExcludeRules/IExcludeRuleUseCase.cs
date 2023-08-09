namespace Application.FormRules.Usecases.ExcludeRules;

public interface IExcludeRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}