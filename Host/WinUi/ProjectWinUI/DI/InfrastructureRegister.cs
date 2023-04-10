using Microsoft.Extensions.DependencyInjection;

namespace ProjectWinUI.DI;

public static class InfrastructureRegister
{
    public static IServiceCollection RegisterInfrastructure(
        this IServiceCollection service
    ) => service;
}