using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;

namespace ProjectAvalonia.Common.Helpers;

public static class NotificationHelpers
{
    private const int DefaultNotificationTimeout = 10;
    private static WindowNotificationManager? _notificationManager;

    public static void SetNotificationManager(
        Window host
    ) =>
        _notificationManager = new WindowNotificationManager(host: TopLevel.GetTopLevel(host))
        {
            Position = NotificationPosition.BottomRight, MaxItems = 4
            , Margin = new Thickness(left: 0, top: 0, right: 15, bottom: 40)
        };

    public static void Show(
        string title
        , string message
        , Action? onClick = null
    )
    {
        if (_notificationManager is { } nm)
        {
            Dispatcher.UIThread.Post(() =>
                nm.Show(
                    content: new Notification(title: title
                        , message: message, type: NotificationType.Information
                        , expiration: TimeSpan.FromSeconds(value: DefaultNotificationTimeout), onClick: onClick)));
        }
    }

    public static void Show(
        string title
        , string message
        , int time
        , Action? onClick = null
    ) =>
        _notificationManager?.Show(
            content: new Notification(title: title
                , message: message, type: NotificationType.Information
                , expiration: TimeSpan.FromSeconds(value: time), onClick: onClick));

    public static void ShowMain(
        string title
        , string message
        , int time
        , NotificationType type = NotificationType.Information
        , Action? onClick = null
    ) =>
        _notificationManager?.Show(
            content: new Notification(
                title: title
                , message: message, type: type
                , expiration: TimeSpan.FromSeconds(value: time), onClick: onClick));

    public static void Show(
        object viewModel
    ) => _notificationManager?.Show(content: viewModel);
}