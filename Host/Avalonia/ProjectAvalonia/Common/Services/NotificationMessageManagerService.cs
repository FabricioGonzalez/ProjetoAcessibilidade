using System.Diagnostics;
using Avalonia.Notification;
using ProjetoAcessibilidade.Domain.App.Contracts;

namespace ProjectAvalonia.Services;

public class NotificationMessageManagerService : INotificationMessageManagerService
{
    public INotificationMessageManager Manager
    {
        get;
    } = new NotificationMessageManager();

    public void ShowDebug(
        string message
    ) => /* Manager
                .CreateMessage()
                .Accent("#1751C3")
                .Animates(true)
                .Background("#333")
                .HasBadge("Info")
                .HasMessage(message)
                .Dismiss()
                    .WithDelay(TimeSpan.FromSeconds(5))
                */ /*.Dismiss().WithButton("Update now", button => { })
                .Dismiss().WithButton("Release notes", button => { })
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))*/ /*
                .Queue()*/Debug.WriteLine(message: $"Debug {message}");

    public void ShowError(
        string message
    ) => /*Manager
                .CreateMessage()
                .Accent("#1751C3")
                .Animates(true)
                .Background("#333")
                .HasBadge("Info")
                .HasMessage(message)
                .Dismiss()
                    .WithDelay(TimeSpan.FromSeconds(5))
                */ /*.Dismiss().WithButton("Update now", button => { })
                .Dismiss().WithButton("Release notes", button => { })
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))*/ /*
                .Queue();*/Debug.WriteLine(message: $"Error {message}");

    public void ShowInfo(
        string message
    ) => /*Manager
                .CreateMessage()
                .Accent("#1751C3")
                .Animates(true)
                .Background("#333")
                .HasBadge("Info")
                .HasMessage(message)
                .Dismiss()
                    .WithDelay(TimeSpan.FromSeconds(5))
                */ /*.Dismiss().WithButton("Update now", button => { })
                .Dismiss().WithButton("Release notes", button => { })
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))*/ /*
                .Queue();*/ Debug.WriteLine(message: $"Info {message}");

    public void ShowWarning(
        string message
    ) => /*Manager
                .CreateMessage()
                .Accent("#1751C3")
                .Animates(true)
                .Background("#333")
                .HasBadge("Info")
                .HasMessage(message)
                .Dismiss()
                    .WithDelay(TimeSpan.FromSeconds(5))
                */ /*.Dismiss().WithButton("Update now", button => { })
                .Dismiss().WithButton("Release notes", button => { })
                .Dismiss().WithDelay(TimeSpan.FromSeconds(5))*/ /*
                .Queue();*/ Debug.WriteLine(message: $"Warning {message}");
}