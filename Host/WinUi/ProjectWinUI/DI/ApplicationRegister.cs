using Microsoft.Extensions.DependencyInjection;

namespace ProjectWinUI.DI;

public static class ApplicationRegister
{
    public static IServiceCollection RegisterApplication(
        this IServiceCollection service
    ) => service;
}