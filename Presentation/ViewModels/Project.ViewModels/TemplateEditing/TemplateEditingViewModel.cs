using System.Reactive.Disposables;

using App.Core.Entities.Solution.Explorer;

using AppViewModels.Common;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.TemplateEditing;
public class TemplateEditingViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "TemplateEditing";

    private ObservableCollectionExtended<ExplorerItem> projectItems;

    public ObservableCollectionExtended<ExplorerItem> ProjectItems
    {
        get => projectItems;
        set => this.RaiseAndSetIfChanged(ref projectItems, value);

    }


    public TemplateEditingViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) => {
            
        });
    }
}
