using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;

using Project.Core.ViewModels.Extensions;

using ReactiveUI;

using Splat;

using UIStatesStore.Contracts;
using UIStatesStore.Project.Models;

namespace Project.Core.ViewModels.Project;
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

        CloseItemCommand = ReactiveCommand.Create<ProjectEditingModel>((item) =>
        {
            if (item is not null)
            {
                Items.Remove(item);
            }
        });

        this.WhenActivated(disposables =>
        {
            ProjectEditingObservable.Subscribe(item =>
            {
                /*  if (!Items.Any(registeredItem => true ))*/
                Items.Add(item);
            })
           .DisposeWith(disposables);
        });
    }
    public ReactiveCommand<ProjectEditingModel, Unit> CloseItemCommand
    {
        get; private set;
    }

}
