using System;
using System.IO;
using System.Threading.Tasks;

using ProjetoAcessibilidade.Contracts.Services;
using ProjetoAcessibilidade.ViewModels;

using SystemApplication.Services.ProjectDataServices;

using Windows.ApplicationModel.Activation;

namespace ProjetoAcessibilidade.Activation;
public class FileActivationHandler : ActivationHandler<FileActivatedEventArgs>
{
    readonly INavigationService _navigationService;
    readonly GetProjectData _getProjectData;
    public FileActivationHandler(INavigationService navigationService, GetProjectData getProjectData)
    {
        _navigationService = navigationService;
        _getProjectData = getProjectData;
    }

    protected override bool CanHandleInternal(FileActivatedEventArgs args)
    {
        return args.Files.Count > 0;
    }

    protected async override Task HandleInternalAsync(FileActivatedEventArgs args)
    {
        var solution = await _getProjectData.GetProjectSolution(args.Files[0].Path);
        _navigationService.NavigateTo(typeof(ProjectViewModel).FullName, solution);

        await Task.CompletedTask;
    }
}
