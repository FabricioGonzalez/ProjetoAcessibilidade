using System.Threading.Tasks;

using ProjectWinUI.Src.Activation.Contracts;

namespace ProjectWinUI.Src.Activation.Handlers;
public abstract class ActivationHandler<T> : IActivationHandler
    where T : class
{
    // Override this method to add the logic for whether to handle the activation.
    protected virtual bool CanHandlepublic(T args) => true;

    // Override this method to add the logic for your activation handler.
    protected abstract Task HandlepublicAsync(T args);

    public bool CanHandle(object args) => args is T && CanHandlepublic((args as T)!);

    public async Task HandleAsync(object args) => await HandlepublicAsync((args as T)!);
}
