using System.Reactive.Disposables;

using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.TemplateEditing;
public class TemplateEditingViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "TemplateEditing";

    public TemplateEditingViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) => { });
    }
}
