using System;
using Avalonia.Threading;

namespace ProjectAvalonia.Common.Controls.Spectrum;

public abstract class SpectrumDataSource
{
    private readonly float[] _averaged;
    private readonly DispatcherTimer _timer;

    public SpectrumDataSource(
        int numBins
        , int numAverages
        , TimeSpan mixInterval
    )
    {
        Bins = new float[numBins];
        _averaged = new float[numBins];

        _timer = new DispatcherTimer
        {
            Interval = mixInterval
        };

        _timer.Tick += TimerOnTick;

        NumAverages = numAverages;
    }

    public int NumAverages
    {
        get;
    }

    protected float[] Bins
    {
        get;
    }

    protected int NumBins => Bins.Length;

    protected int MidPointBins => NumBins / 2;

    public event EventHandler<bool>? GeneratingDataStateChanged;

    private void TimerOnTick(
        object? sender
        , EventArgs e
    ) => OnMixData();

    protected abstract void OnMixData();

    public void Render(
        ref float[] data
    )
    {
        for (var i = 0; i < NumBins; i++)
        {
            _averaged[i] -= _averaged[i] / NumAverages;
            _averaged[i] += Bins[i] / NumAverages;

            data[i] = Math.Max(val1: data[i], val2: _averaged[i]);
        }
    }

    public void Start()
    {
        _timer.Start();
        OnGeneratingDataStateChanged(isGenerating: true);
    }

    public void Stop()
    {
        _timer.Stop();
        OnGeneratingDataStateChanged(isGenerating: false);
    }

    private void OnGeneratingDataStateChanged(
        bool isGenerating
    ) => GeneratingDataStateChanged?.Invoke(sender: this, e: isGenerating);
}