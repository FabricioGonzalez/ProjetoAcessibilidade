namespace Application.Usecases.FormRules.EditRules;

public interface IEditRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}