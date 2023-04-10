using System.Threading.Tasks;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using ProjectWinUI.Src.Navigation.Contracts;
using ProjectWinUI.Src.Notifications.Contracts;

namespace ProjectWinUI.Src.Activation.Handlers;

public class AppNotificationActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;
    private readonly IAppNotificationService _notificationService;

    public AppNotificationActivationHandler(
        INavigationService navigationService
        , IAppNotificationService notificationService
    )
    {
        _navigationService = navigationService;
        _notificationService = notificationService;
    }

    protected override bool CanHandlepublic(
        LaunchActivatedEventArgs args
    ) => AppInstance.GetCurrent().GetActivatedEventArgs()?.Kind == ExtendedActivationKind.AppNotification;

    protected async override Task HandlepublicAsync(
        LaunchActivatedEventArgs args
    )
    {
        // TODO: Handle notification activations.

        //// // Access the AppNotificationActivatedEventArgs.
        //// var activatedEventArgs = (AppNotificationActivatedEventArgs)AppInstance.GetCurrent().GetActivatedEventArgs().Data;

        //// // Navigate to a specific page based on the notification arguments.
        //// if (_notificationService.ParseArguments(activatedEventArgs.Argument)["action"] == "Settings")
        //// {
        ////     // Queue navigation with low priority to allow the UI to initialize.
        ////     App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low, () =>
        ////     {
        ////         _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        ////     });
        //// }

        WinApp.MainWindow.DispatcherQueue.TryEnqueue(priority: DispatcherQueuePriority.Low, callback: () =>
        {
            WinApp.MainWindow.ShowMessageDialogAsync(content: "TODO: Handle notification activations."
                , title: "Notification Activation");
        });

        await Task.CompletedTask;
    }
}