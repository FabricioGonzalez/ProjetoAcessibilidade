using System.Collections.ObjectModel;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.Models;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class SolutionState : ReactiveObject
{
    private readonly ILocationService _locationService;
    private string _fileName = "";
    private string _filePath = "";

    private ObservableCollection<LocationItemState> _locationItems = new();

    private string _logoPath = "";

    private SolutionReportState _report = new();

    public SolutionState(
        ILocationService locationService
    )
    {
        _locationService = locationService;
    }

    public SearchSolutionTexts SearchSolutionTexts
    {
        get;
    } = new();

    public ReactiveCommand<int, Unit> LoadCities => ReactiveCommand.CreateRunInBackground<int>(uf =>
    {
        SearchSolutionTexts.Cidades = new ObservableCollection<Cidade>(_locationService.GetCidades(uf));
    });

    public ReactiveCommand<Unit, Unit> LoadUFs => ReactiveCommand.CreateRunInBackground(() =>
    {
        SearchSolutionTexts.UFs = new ObservableCollection<Uf>(_locationService.GetAllUfs());
    });

    public SolutionReportState Report
    {
        get => _report;
        set => this.RaiseAndSetIfChanged(backingField: ref _report, newValue: value);
    }

    public string LogoPath
    {
        get => _logoPath;
        set => this.RaiseAndSetIfChanged(backingField: ref _logoPath, newValue: value);
    }

    public string FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(backingField: ref _filePath, newValue: value);
    }

    public string FileName
    {
        get => _fileName;
        set => this.RaiseAndSetIfChanged(backingField: ref _fileName, newValue: value);
    }

    public ObservableCollection<LocationItemState> LocationItems
    {
        get => _locationItems;
        set => this.RaiseAndSetIfChanged(backingField: ref _locationItems, newValue: value);
    }
}

public sealed class SearchSolutionTexts : ReactiveObject
{
    private ObservableCollection<Cidade> _cidades = new();
    private string _searchCidade = "";
    private string _searchUf = "";

    private ObservableCollection<Uf> _ufs = new();

    public string SearchUf
    {
        get => _searchUf;
        set => this.RaiseAndSetIfChanged(backingField: ref _searchUf, newValue: value);
    }

    public ObservableCollection<Cidade> Cidades
    {
        get => _cidades;
        set => this.RaiseAndSetIfChanged(backingField: ref _cidades, newValue: value);
    }

    public ObservableCollection<Uf> UFs
    {
        get => _ufs;
        set => this.RaiseAndSetIfChanged(backingField: ref _ufs, newValue: value);
    }

    public string SearchCidade
    {
        get => _searchCidade;
        set => this.RaiseAndSetIfChanged(backingField: ref _searchCidade, newValue: value);
    }
}