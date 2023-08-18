using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public sealed class SolutionReportState : ReactiveObject
{
    private CompanyInfoState _companyInfo = new();
    private ManagementCompanyInfoState _managerInfo = new();

    private ObservableCollection<PartnerLogoState> _partners = new();

    private string _solutionName = "";

    public string SolutionName
    {
        get => _solutionName;
        set => this.RaiseAndSetIfChanged(backingField: ref _solutionName, newValue: value);
    }

    public CompanyInfoState CompanyInfo
    {
        get => _companyInfo;
        set => this.RaiseAndSetIfChanged(backingField: ref _companyInfo, newValue: value);
    }

    public ManagementCompanyInfoState ManagerInfo
    {
        get => _managerInfo;
        set => this.RaiseAndSetIfChanged(backingField: ref _managerInfo, newValue: value);
    }

    public ObservableCollection<PartnerLogoState> Partners
    {
        get => _partners;
        set => this.RaiseAndSetIfChanged(backingField: ref _partners, newValue: value);
    }

    public ReactiveCommand<Unit, Unit> AddItemToPartnerCommand =>
        ReactiveCommand.Create(() => Partners.Add(new PartnerLogoState()));
}