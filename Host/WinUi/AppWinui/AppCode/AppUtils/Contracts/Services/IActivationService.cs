namespace AppWinui.AppCode.AppUtils.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
