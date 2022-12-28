using Microsoft.Extensions.DependencyInjection;

using ProjectWinUI.Src.Activation.Contracts;
using ProjectWinUI.Src.Activation.Services;
using ProjectWinUI.Src.App.Contracts;
using ProjectWinUI.Src.App.Services;
using ProjectWinUI.Src.Navigation.Contracts;
using ProjectWinUI.Src.Navigation.Services;
using ProjectWinUI.Src.Notifications.Contracts;
using ProjectWinUI.Src.Notifications.Services;
using ProjectWinUI.Src.Settings.Contracts;
using ProjectWinUI.Src.Settings.Services;
using ProjectWinUI.Src.Theming.Contracts;
using ProjectWinUI.Src.Theming.Services;

namespace ProjectWinUI.DI;
public static class InternalServiceRegister
{
    public static IServiceCollection RegisterServices(this IServiceCollection service)
    {
        service.AddSingleton<IAppNotificationService, AppNotificationService>();
        service.AddSingleton<INavigationService, NavigationService>();
        service.AddSingleton<INavigationViewService, NavigationViewService>();
        service.AddSingleton<IPageService, PageService>();
        service.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        service.AddSingleton<IActivationService, ActivationService>();
        service.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        service.AddSingleton<IFileService, FileService>();

        return service;
    }
}
