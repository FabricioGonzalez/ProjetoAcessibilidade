using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using ProjectAvalonia.Common.Bases;
using ProjectAvalonia.Common.Models;

namespace ProjectAvalonia.Common.Services;

public class UpdateChecker : PeriodicRunner
{
    public UpdateChecker(
        TimeSpan period
    ) : base(period: period)
    {
        UpdateStatus = new UpdateStatus(clientUpToDate: true, clientVersion: new Version());
        /* Synchronizer.PropertyChanged += Synchronizer_PropertyChanged; */
    }

    public UpdateStatus UpdateStatus
    {
        get;
        private set;
    }

    public AppClient AppClient
    {
        get;
        set;
    }

    public event EventHandler<UpdateStatus>? UpdateStatusChanged;

    private void Synchronizer_PropertyChanged(
        object? sender
        , PropertyChangedEventArgs e
    )
    {
        /* if (e.PropertyName == nameof(WasabiSynchronizer.BackendStatus) &&
            Synchronizer.BackendStatus == BackendStatus.Connected)
        {
            // Any time when the synchronizer detects the backend, we immediately check the versions. GUI relies on UpdateStatus changes.
            TriggerRound();
        } */
    }

    protected async override Task ActionAsync(
        CancellationToken cancel
    )
    {
        var newUpdateStatus = await AppClient.CheckUpdatesAsync(cancel: cancel)
            .ConfigureAwait(continueOnCapturedContext: false);
        if (newUpdateStatus != UpdateStatus)
        {
            UpdateStatus = newUpdateStatus;
            UpdateStatusChanged?.Invoke(sender: this, e: newUpdateStatus);
        }
    }

    public override void Dispose() =>
        /*  Synchronizer.PropertyChanged -= Synchronizer_PropertyChanged; */
        base.Dispose();
}