using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;

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

    private SolutionReportState _report;

    public SolutionState(
        ILocationService locationService,
        IFilePickerService fileService
    )
    {
        _locationService = locationService;

        _report = new(fileService);

        this.WhenAnyValue(it => it.Report.CompanyInfo.Endereco.Uf)
            .Where(it => !string.IsNullOrWhiteSpace(it))
            .InvokeCommand(LoadCities);

        this.WhenAnyValue(it => it.SearchSolutionTexts.SelectedUf)
            .WhereNotNull()
            .Subscribe(changedUf =>
            {
                Report.CompanyInfo.Endereco.Uf = changedUf.ShortName;

            });
    }

    public SearchSolutionTexts SearchSolutionTexts
    {
        get;
    } = new();

    public ReactiveCommand<string, Unit> LoadCities => ReactiveCommand.CreateRunInBackground<string>(uf =>
    {
        var UfData = _locationService.GetUfByName(uf);

        SearchSolutionTexts.Cidades = new ObservableCollection<Cidade>(
            _locationService
            .GetCidades(
                UfData.Code
                )
            );

        if (Report.CompanyInfo.Endereco.Cidade is { } cidade && Report.CompanyInfo.Endereco.Uf is { } UF)
        {
            SearchSolutionTexts.SelectedCidade = _locationService
            .GetCidades(_locationService.GetUfByName(UF).Code)
            .FirstOrDefault(it => it.Nome == cidade);

        }
    });

    public ReactiveCommand<Unit, Unit> LoadUFs => ReactiveCommand.CreateRunInBackground(() =>
    {
        SearchSolutionTexts.UFs = new ObservableCollection<Uf>(_locationService.GetAllUfs());

        if (Report.CompanyInfo.Endereco.Uf is { } uf)
        {
            SearchSolutionTexts.SelectedUf = _locationService.GetUfByName(uf);
        }

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
    private Cidade _selectedCidade;
    private Uf _selectedUf;
    private string _searchCidade = "";
    private string _searchUf = "";


    private ObservableCollection<Uf> _ufs = new();
    public Cidade SelectedCidade
    {
        get => _selectedCidade;
        set => this.RaiseAndSetIfChanged(backingField: ref _selectedCidade, newValue: value);
    }

    public Uf SelectedUf
    {
        get => _selectedUf;
        set => this.RaiseAndSetIfChanged(backingField: ref _selectedUf, newValue: value);
    }
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