using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ProjectAvalonia.Common.Controls;
using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.Presentation.States;

namespace ProjectAvalonia.Features.Project.Components;

public partial class SolutionItemContent : UserControl
{
    /*public static readonly DirectProperty<SolutionItemContent, IEnumerable<Cidade>?> CidadesProperty =
        AvaloniaProperty.RegisterDirect<SolutionItemContent, IEnumerable<Cidade>?>(name: nameof(Cidades)
            , getter: o => o.Cidades, setter: (
                content
                , cidades
            ) => content.Cidades = cidades);

    public static readonly DirectProperty<SolutionItemContent, IEnumerable<Uf>?> UfProperty =
        AvaloniaProperty.RegisterDirect<SolutionItemContent, IEnumerable<Uf>?>(name: nameof(Ufs)
            , getter: o => o.Ufs, setter: (
                content
                , ufs
            ) => content.Ufs = ufs);

    private readonly LocationService _location = new();

    private IEnumerable<Cidade>? _cidades;

    private IEnumerable<Uf>? _ufs;*/

    public SolutionItemContent()
    {
        InitializeComponent();

        /*StateContainer.WhenAnyValue(it => it.SelectedItem)
            .WhereNotNull()
            .Subscribe(it =>
            {
                Cidades = _location.GetCidades(((Uf)it).Code);
                CityContainer.ItemsSource = Cidades;
            });*/
    }

    /*public IEnumerable<Cidade>? Cidades
    {
        get => _cidades;
        set => SetAndRaise(property: CidadesProperty, field: ref _cidades, value: value);
    }

    public IEnumerable<Uf>? Ufs
    {
        get => _ufs;
        set => SetAndRaise(property: UfProperty, field: ref _ufs, value: value);
    }*/

    private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

    private async void PartnerLogoItemPathSelector_OnClick(
        object? sender
        , RoutedEventArgs e
    )
    {
        if (sender is IconButton button)
        {
            (await FileDialogHelper.GetImagesAsync())
                .Match(Succ: s =>
                {
                    ((PartnerLogoState)button.DataContext).Logo = s.Path.LocalPath;

                    return true;
                }, Fail: f =>
                {
                    return false;
                });
        }
    }

    private async void SolutionItemPathSelector_OnClick(
        object? sender
        , RoutedEventArgs e
    ) =>
        (await FileDialogHelper.GetFolderAsync())
        .Match(Succ: s =>
        {
            ((SolutionState)DataContext).FilePath = s.Path.LocalPath;

            ((SolutionState)DataContext).FileName = Path.GetFileName(((SolutionState)DataContext).FilePath);

            return true;
        }, Fail: f =>
        {
            return false;
        });

    private async void CompanyLogoPathSelector_OnClick(
        object? sender
        , RoutedEventArgs e
    ) =>
        (await FileDialogHelper.GetImagesAsync())
        .Match(Succ: s =>
        {
            ((SolutionState)DataContext).Report.CompanyInfo.Logo = s.Path.LocalPath;

            return true;
        }, Fail: f =>
        {
            return false;
        });

    private async void ManagerLogoPathSelector_OnClick(
        object? sender
        , RoutedEventArgs e
    ) =>
        (await FileDialogHelper.GetImagesAsync())
        .Match(Succ: s =>
        {
            ((SolutionState)DataContext).Report.ManagerInfo.LogoPath = s.Path.LocalPath;

            return true;
        }, Fail: f =>
        {
            return false;
        });
}