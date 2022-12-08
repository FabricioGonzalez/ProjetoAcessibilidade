using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

using ProjectWinUI.Src.Activation.Contracts;
using ProjectWinUI.Src.Activation.Handlers;
using ProjectWinUI.Src.Notifications.Contracts;
using ProjectWinUI.Src.Notifications.Services;

namespace ProjectWinUI.DI;
public static class ActivationHandlersRegister
{
    public static IServiceCollection RegisterActivationHandlers(this IServiceCollection services)
    {
        // Default Activation Handler
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        // Other Activation Handlers
        services.AddTransient<IActivationHandler, AppNotificationActivationHandler>();

        // Services
        services.AddSingleton<IAppNotificationService, AppNotificationService>();

        return services;
    }
}
