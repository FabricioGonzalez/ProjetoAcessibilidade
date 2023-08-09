namespace Application.Solution.Usecases.EditRules;

public interface IEditRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}