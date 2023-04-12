using Microsoft.Extensions.Hosting;
using ProjectAvalonia.Common.Helpers;

namespace ProjectAvalonia.Common.Services;

public class HostedService
{
    public HostedService(
        IHostedService service
        , string friendlyName
    )
    {
        Service = Guard.NotNull(parameterName: nameof(service), value: service);
        FriendlyName = Guard.NotNull(parameterName: nameof(friendlyName), value: friendlyName);
    }

    public IHostedService Service
    {
        get;
    }

    public string FriendlyName
    {
        get;
    }
}