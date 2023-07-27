namespace Application.Usecases.FormRules.TransferRules;

public interface ITransferRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}