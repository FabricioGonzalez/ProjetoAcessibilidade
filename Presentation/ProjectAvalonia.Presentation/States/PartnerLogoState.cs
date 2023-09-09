using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public sealed class PartnerLogoState : ReactiveObject
{
    private string _partnerLogo = "";
    private string _partnerName = "";
    private string _website = "";

    public string Name
    {
        get => _partnerName;
        set => this.RaiseAndSetIfChanged(backingField: ref _partnerName, newValue: value);
    }

    public string Logo
    {
        get => _partnerLogo;
        set => this.RaiseAndSetIfChanged(backingField: ref _partnerLogo, newValue: value);
    }

    public string Website
    {
        get => _website;
        set => this.RaiseAndSetIfChanged(backingField: ref _website, newValue: value);
    }
}