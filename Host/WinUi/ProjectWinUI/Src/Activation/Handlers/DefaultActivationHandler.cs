using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using ProjectWinUI.Src.Navigation.Contracts;

namespace ProjectWinUI.Src.Activation.Handlers;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(
        INavigationService navigationService
    )
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandlepublic(
        LaunchActivatedEventArgs args
    ) =>
        // None of the ActivationHandlers has handled the activation.
        _navigationService.Frame?.Content == null;

    protected async override Task HandlepublicAsync(
        LaunchActivatedEventArgs args
    ) =>
        /*_navigationService.NavigateTo(pageKey: typeof().FullName!, parameter: args.Arguments);*/
        await Task.CompletedTask;
}