using System.Threading.Tasks;

namespace ProjectWinUI.Src.Activation.Contracts;

public interface IActivationService
{
    Task ActivateAsync(
        object activationArgs
    );
}