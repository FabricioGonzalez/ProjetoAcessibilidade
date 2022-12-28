namespace Project.Application.App.Contracts;
public interface INotificationMessageManagerService
{
    void ShowInfo(string message);
    void ShowWarning(string message);
    void ShowError(string message);
    void ShowDebug(string message);
}
