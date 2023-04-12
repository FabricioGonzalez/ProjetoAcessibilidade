using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using ProjectAvalonia.Common.Helpers;
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

    protected override void OnNavigatedTo(
        bool isInHistory
        , CompositeDisposable disposables
    )
    {
        base.OnNavigatedTo(isInHistory: isInHistory, disposables: disposables);

        RxApp.MainThreadScheduler.Schedule(action: async () =>
        {
            await Task.Delay(millisecondsDelay: 500);
            ThemeHelper.ApplyTheme(theme: _newTheme);
            Navigate().Clear();
        });
    }
}