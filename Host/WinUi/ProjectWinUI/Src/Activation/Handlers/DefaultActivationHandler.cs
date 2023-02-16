using System.Threading.Tasks;

using AppViewModels.Project;

using Microsoft.UI.Xaml;

<<<<<<< HEAD:ProjetoAcessibilidade/Activation/DefaultActivationHandler.cs
using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.Modules.Home.ViewModels;

namespace ProjetoAcessibilidade.Activation;
=======
using ProjectWinUI.Src.Navigation.Contracts;
>>>>>>> reinit:Host/WinUi/ProjectWinUI/Src/Activation/Handlers/DefaultActivationHandler.cs

namespace ProjectWinUI.Src.Activation.Handlers;
public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandlepublic(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandlepublicAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(ProjectViewModel).FullName!, args.Arguments);

        await Task.CompletedTask;
    }
}
