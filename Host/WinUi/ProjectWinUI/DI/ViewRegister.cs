using Microsoft.Extensions.DependencyInjection;
using ProjectWinUI.Src.Navigation;
using ProjectWinUI.Src.Project;

namespace ProjectWinUI.DI;

public static class ViewRegister
{
    public static IServiceCollection RegisterViews(
        this IServiceCollection service
    )
    {
        service.AddTransient<ShellPage>();
        service.AddTransient<ProjectPage>();

        return service;
    }

    public static IServiceCollection RegisterPages(
        this IServiceCollection service
    ) => service;
}