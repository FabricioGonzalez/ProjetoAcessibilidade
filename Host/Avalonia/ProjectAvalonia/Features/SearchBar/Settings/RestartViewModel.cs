using System.Windows.Input;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.ViewModels;
using ReactiveUI;

namespace ProjectAvalonia.Features.SearchBar.Settings;

public class RestartViewModel : ViewModelBase
{
    public RestartViewModel(
        string message
    )
    {
        Message = message;
        RestartCommand = ReactiveCommand.Create(execute: () =>
            AppLifetimeHelper.Shutdown(withShutdownPrevention: true, restart: true));
    }

    public string Message
    {
        get;
    }

    public ICommand RestartCommand
    {
        get;
    }
}