using System.Collections.Specialized;
using System.Web;
using Microsoft.Windows.AppNotifications;
using ProjectWinUI.Src.Navigation.Contracts;
using ProjectWinUI.Src.Notifications.Contracts;

namespace ProjectWinUI.Src.Notifications.Services;

public class AppNotificationService : IAppNotificationService
{
    private readonly INavigationService _navigationService;

    public AppNotificationService(
        INavigationService navigationService
    )
    {
        _navigationService = navigationService;
    }

    public void Initialize()
    {
        AppNotificationManager.Default.NotificationInvoked += OnNotificationInvoked;

        AppNotificationManager.Default.Register();
    }

    public bool Show(
        string payload
    )
    {
        var appNotification = new AppNotification(payload: payload);

        AppNotificationManager.Default.Show(notification: appNotification);

        return appNotification.Id != 0;
    }

    public NameValueCollection ParseArguments(
        string arguments
    ) => HttpUtility.ParseQueryString(query: arguments);

    public void Unregister() => AppNotificationManager.Default.Unregister();

    ~AppNotificationService()
    {
        Unregister();
    }

    public void OnNotificationInvoked(
        AppNotificationManager sender
        , AppNotificationActivatedEventArgs args
    ) =>
        // TODO: Handle notification invocations when your app is already running.
        //// // Navigate to a specific page based on the notification arguments.
        //// if (ParseArguments(args.Argument)["action"] == "Settings")
        //// {
        ////    App.MainWindow.DispatcherQueue.TryEnqueue(() =>
        ////    {
        ////        _navigationService.NavigateTo(typeof(SettingsViewModel).FullName!);
        ////    });
        //// }
        WinApp.MainWindow.DispatcherQueue.TryEnqueue(callback: () =>
        {
            WinApp.MainWindow.ShowMessageDialogAsync(
                content: "TODO: Handle notification invocations when your app is already running.",
                title: "Notification Invoked");

            WinApp.MainWindow.BringToFront();
        });
}