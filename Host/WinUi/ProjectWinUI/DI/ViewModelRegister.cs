using Microsoft.Extensions.DependencyInjection;
using ProjectWinUI.Navigation;

namespace ProjectWinUI.DI;

public static class ViewModelRegister
{
    public static IServiceCollection RegisterViewModel(
        this IServiceCollection service
    )
    {
        service.AddTransient<ShellViewModel>();


        return service;
    }
}