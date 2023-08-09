namespace Application.FormRules.Usecases.EditRules;

public interface IEditRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}