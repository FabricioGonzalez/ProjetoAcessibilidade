using System;
using System.Threading;
using System.Threading.Tasks;

using ProjectAvalonia.Common.Bases;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Services;

public class UpdateChecker : PeriodicRunner
{
    public UpdateChecker(TimeSpan period) : base(period)
    {
        UpdateStatus = new UpdateStatus(true, true, new Version(), 0, new Version());

        /*Synchronizer.PropertyChanged += Synchronizer_PropertyChanged;*/
    }

    public event EventHandler<UpdateStatus>? UpdateStatusChanged;

    public UpdateStatus UpdateStatus
    {
        get; private set;
    }
    /*    public AppClient AppClient
        {
            get;
        }*/

    protected override async Task ActionAsync(CancellationToken cancel)
    {
        /*        var newUpdateStatus = await AppClient.CheckUpdatesAsync(cancel).ConfigureAwait(false);
                if (newUpdateStatus != UpdateStatus)
                {
                    UpdateStatus = newUpdateStatus;
                    UpdateStatusChanged?.Invoke(this, newUpdateStatus);
                }*/
    }

    public override void Dispose()
    {
        /*Synchronizer.PropertyChanged -= Synchronizer_PropertyChanged;*/
        base.Dispose();
    }
}
