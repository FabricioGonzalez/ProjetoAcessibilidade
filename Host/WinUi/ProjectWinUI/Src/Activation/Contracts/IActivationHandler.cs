using System.Threading.Tasks;

namespace ProjectWinUI.Src.Activation.Contracts;
public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
