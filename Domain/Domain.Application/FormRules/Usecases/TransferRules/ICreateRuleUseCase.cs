namespace Application.FormRules.Usecases.TransferRules;

public interface ITransferRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}