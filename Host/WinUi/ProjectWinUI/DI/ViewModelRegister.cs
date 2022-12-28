using AppViewModels.Project;

using Microsoft.Extensions.DependencyInjection;

using ProjectWinUI.Src.Navigation;

namespace ProjectWinUI.DI;
public static class ViewModelRegister
{
    public static IServiceCollection RegisterViewModel(this IServiceCollection service)
    {

        service.AddTransient<ShellViewModel>();
        service.AddTransient<ProjectViewModel>();

        return service;
    }
}
