using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ProjectWinUI.Src.Activation.Contracts;
using ProjectWinUI.Src.Activation.Handlers;
using ProjectWinUI.Src.Navigation;
using ProjectWinUI.Src.Theming.Contracts;

namespace ProjectWinUI.Src.Activation.Services;

public class ActivationService : IActivationService
{
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IThemeSelectorService _themeSelectorService;
    private UIElement? _shell;

    public ActivationService(
        ActivationHandler<LaunchActivatedEventArgs> defaultHandler
        , IEnumerable<IActivationHandler> activationHandlers
        , IThemeSelectorService themeSelectorService
    )
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
    }

    public async Task ActivateAsync(
        object activationArgs
    )
    {
        // Execute tasks before activation.
        await InitializeAsync();

        // Set the MainWindow Content.
        if (WinApp.MainWindow.Content == null)
        {
            _shell = WinApp.GetService<ShellPage>();
            WinApp.MainWindow.Content = _shell ?? new Frame();
        }

        // Handle activation via ActivationHandlers.
        await HandleActivationAsync(activationArgs: activationArgs);

        // Activate the MainWindow.
        WinApp.MainWindow.Activate();

        // Execute tasks after activation.
        await StartupAsync();
    }

    private async Task HandleActivationAsync(
        object activationArgs
    )
    {
        var activationHandler = _activationHandlers.FirstOrDefault(predicate: h => h.CanHandle(args: activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(args: activationArgs);
        }

        if (_defaultHandler.CanHandle(args: activationArgs))
        {
            await _defaultHandler.HandleAsync(args: activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        await _themeSelectorService.InitializeAsync().ConfigureAwait(continueOnCapturedContext: false);
        await Task.CompletedTask;
    }

    private async Task StartupAsync()
    {
        await _themeSelectorService.SetRequestedThemeAsync();
        await Task.CompletedTask;
    }
}