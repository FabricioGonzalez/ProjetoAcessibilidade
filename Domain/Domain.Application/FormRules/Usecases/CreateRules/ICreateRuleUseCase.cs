namespace Application.FormRules.Usecases.CreateRules;

public interface ICreateRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}