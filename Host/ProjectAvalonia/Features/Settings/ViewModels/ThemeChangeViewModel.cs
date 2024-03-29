using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using ProjectAvalonia.Common.Helpers;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Navigation;

using ReactiveUI;

namespace ProjectAvalonia.Features.Settings.ViewModels;

[NavigationMetaData(Title = "")]
public partial class ThemeChangeViewModel : RoutableViewModel
{
    private readonly Theme _newTheme;

    public ThemeChangeViewModel(
        Theme newTheme
    )
    {
        _newTheme = newTheme;
    }


    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;
    public override MenuViewModel? ToolBar => null;
    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
    )
    {
        base.OnNavigatedTo(isInHistory, disposables);

        RxApp.MainThreadScheduler.Schedule(async () =>
        {
            await Task.Delay(500);
            ThemeHelper.ApplyTheme(_newTheme);
            Navigate().Clear();
        });
    }
}