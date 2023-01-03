using System.Reactive.Disposables;

using App.Core.Entities.Solution.Project.AppItem;

using AppViewModels.Common;
using AppViewModels.Project.ComposableViewModels;

using ReactiveUI;

namespace AppViewModels.Project;
public class ProjectItemEditingViewModel : ViewModelBase
{
    private AppItemModel item;
    public AppItemModel Item
    {
        get => item;
        set => this.RaiseAndSetIfChanged(ref item, value, nameof(Item));
    }

    private FileProjectItemViewModel selectedItem;
    public FileProjectItemViewModel SelectedItem
    {
        get => selectedItem;
        set => this.RaiseAndSetIfChanged(ref selectedItem, value, nameof(SelectedItem));
    }

    public ProjectItemEditingViewModel()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });
    }
}
