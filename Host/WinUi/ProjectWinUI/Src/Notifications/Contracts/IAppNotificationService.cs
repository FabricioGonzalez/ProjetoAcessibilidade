using System.Collections.Specialized;

namespace ProjectWinUI.Src.Notifications.Contracts;

public interface IAppNotificationService
{
    void Initialize();

    bool Show(
        string payload
    );

    NameValueCollection ParseArguments(
        string arguments
    );

    void Unregister();
}