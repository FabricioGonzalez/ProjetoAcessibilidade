namespace Application.Solution.Usecases.TransferRules;

public interface ITransferRuleUseCase
{
    Task Execute();

    void SetOutputPort(
        IOutputPort outputPort
    );
}