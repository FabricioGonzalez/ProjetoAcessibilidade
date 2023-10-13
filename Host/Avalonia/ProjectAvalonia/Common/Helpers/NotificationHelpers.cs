using System;
using System.Reactive.Concurrency;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;

using ReactiveUI;

namespace ProjectAvalonia.Common.Helpers;

public static class NotificationHelpers
{
    private const int DefaultNotificationTimeout = 10;
    private static WindowNotificationManager? NotificationManager;

    public static void SetNotificationManager(
        Window host
    )
    {
        var notificationManager = new WindowNotificationManager(host: host)
        {
            Position = NotificationPosition.BottomRight,
            MaxItems = 4
            ,
            Margin = new Thickness(left: 0, top: 0, right: 15, bottom: 40)
        };

        NotificationManager = notificationManager;
    }

    public static void Show(
        string title
        , string message
        , Action? onClick = null
    )
    {
        if (NotificationManager is { } nm)
        {
            RxApp.MainThreadScheduler.Schedule(action: () => nm.Show(content: new Notification(title: title
                , message: message, type: NotificationType.Information
                , expiration: TimeSpan.FromSeconds(value: DefaultNotificationTimeout), onClick: onClick)));
        }
    }

    public static void Show(
        string title
        , string message,
        int time
        , Action? onClick = null
    )
    {
        if (NotificationManager is { } nm)
        {
            RxApp.MainThreadScheduler.Schedule(action: () => nm.Show(content: new Notification(title: title
                , message: message, type: NotificationType.Information
                , expiration: TimeSpan.FromSeconds(value: time), onClick: onClick)));
        }
    }

    public static void Show(
        object viewModel
    ) => NotificationManager?.Show(content: viewModel);
}