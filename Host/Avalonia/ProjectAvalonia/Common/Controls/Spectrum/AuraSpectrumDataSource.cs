using System;
using System.Linq;

namespace ProjectAvalonia.Common.Controls.Spectrum;

public class AuraSpectrumDataSource : SpectrumDataSource
{
    private readonly Random _random;

    public AuraSpectrumDataSource(
        int numBins
    ) : base(numBins: numBins, numAverages: 30, mixInterval: TimeSpan.FromSeconds(value: 0.2))
    {
        _random = new Random(Seed: DateTime.Now.Millisecond);
    }

    public bool IsActive
    {
        get;
        set;
    }

    protected override void OnMixData()
    {
        for (var i = 0; i < NumBins; i++)
        {
            Bins[i] = IsActive ? _random.NextSingle() : Bins[i] - 0.1F;
        }

        if (!IsActive && Bins.All(predicate: f => f <= 0))
        {
            Stop();
        }
    }
}