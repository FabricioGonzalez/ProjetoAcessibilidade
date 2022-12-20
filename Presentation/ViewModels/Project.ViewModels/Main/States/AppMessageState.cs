using ReactiveUI;

namespace AppViewModels.Main.States;
public class AppMessageState : ReactiveObject
{
    private string message = "";
    public string Message
    {
        get => message;
        set => this.RaiseAndSetIfChanged(ref message, value, nameof(Message));
    }
}
