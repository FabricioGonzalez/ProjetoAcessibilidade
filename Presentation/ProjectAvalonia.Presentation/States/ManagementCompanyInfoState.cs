using System.Reactive;
using System.Reactive.Linq;

using ProjectAvalonia.Presentation.Interfaces.Services;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.States;

public class ManagementCompanyInfoState : ReactiveObject
{
    private readonly IFilePickerService service;
    private string _email = "";

    private string _logoPath = "";

    private string _nomeEmpresa = "";

    private string _responsavel = "";

    private string _telefone = "";

    private string _website = "";

    private DateTime _reportDate = DateTime.Now;
    public ReactiveCommand<Unit, Unit> SelectLogoCompanyCommand => ReactiveCommand.Create(() =>
    {
        Observable
        .FromAsync(service.GetImagesAsync)
        .Where(it => !string.IsNullOrWhiteSpace(it))
            .Subscribe(val => LogoPath = val,
                error => Console.WriteLine(error));
    });
    public ManagementCompanyInfoState(IFilePickerService service)
    {
        this.service = service;
    }


    public DateTime ReportDate
    {
        get => _reportDate = DateTime.Now;
        set => this.RaiseAndSetIfChanged(ref _reportDate, value);
    }

    public string Telefone
    {
        get => _telefone;
        set => this.RaiseAndSetIfChanged(backingField: ref _telefone, newValue: value);
    }

    public string LogoPath
    {
        get => _logoPath;
        set => this.RaiseAndSetIfChanged(backingField: ref _logoPath, newValue: value);
    }

    public string WebSite
    {
        get => _website;
        set => this.RaiseAndSetIfChanged(backingField: ref _website, newValue: value);
    }

    public string NomeEmpresa
    {
        get => _nomeEmpresa;
        set => this.RaiseAndSetIfChanged(backingField: ref _nomeEmpresa, newValue: value);
    }

    public string Email
    {
        get => _email;
        set => this.RaiseAndSetIfChanged(backingField: ref _email, newValue: value);
    }

    public string Responsavel
    {
        get => _responsavel;
        set => this.RaiseAndSetIfChanged(backingField: ref _responsavel, newValue: value);
    }
}