using System.Collections.ObjectModel;
using System.Reactive.Disposables;

using ReactiveUI;

using Splat;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels;
public class ItemEditingViewModel : ViewModelBase
{
    private ObservableCollection<ProjectEditingModel> items;

    public ObservableCollection<ProjectEditingModel> Items
    {
        get => items;
        set => this.RaiseAndSetIfChanged(ref items, value, nameof(Items));
    }
    readonly IAppObservable<ProjectEditingModel> ProjectEditingObservable;

    public ItemEditingViewModel()
    {
        items = new();
        ProjectEditingObservable = Locator.Current.GetService<IAppObservable<ProjectEditingModel>>();

        this.WhenActivated(disposables =>
        {
            ProjectEditingObservable.Subscribe(item =>
            {
                if (!Items.Any(registeredItem => registeredItem.Equals(item)))
                    Items.Add(item);
            })
           .DisposeWith(disposables);
        });
    }


}
