using System.Collections.ObjectModel;
using System.Reactive;

using ProjectAvalonia.Presentation.Interfaces.Services;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public sealed class SolutionReportState : ReactiveObject
{
    private CompanyInfoState _companyInfo;
    private ManagementCompanyInfoState _managerInfo;

    private ObservableCollection<PartnerLogoState> _partners;

    private int _revisao;
    public int Revisao
    {
        get => _revisao;
        set => this.RaiseAndSetIfChanged(ref _revisao, value);
    }

    public SolutionReportState(IFilePickerService service)
    {
        _companyInfo = new(service);
        _managerInfo = new(service);
        _partners = new();
    }

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