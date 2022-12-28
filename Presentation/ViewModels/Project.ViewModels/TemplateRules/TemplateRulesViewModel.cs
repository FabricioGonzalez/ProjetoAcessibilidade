using System.Reactive.Disposables;

using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.TemplateRules;
public class TemplateRulesViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "TemplateRules";

    public TemplateRulesViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) => { });
    }
}
