namespace Application.Usecases.FormRules.CreateRules;

public interface ICreateRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}