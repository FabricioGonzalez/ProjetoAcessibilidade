using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using AppViewModels.Common;
using AppViewModels.Interactions.Project;
using AppViewModels.Project.ComposableViewModels;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project;
public class ProjectEditingViewModel : ViewModelBase
{
    private ObservableCollection<FileProjectItemViewModel> items;
    public ObservableCollection<FileProjectItemViewModel> Items
    {
        get => items;
        set => this.RaiseAndSetIfChanged(ref items, value, nameof(Items));
    }

    private FileProjectItemViewModel? selectedItem = null;
    public FileProjectItemViewModel SelectedItem
    {
        get => selectedItem;
        set => this.RaiseAndSetIfChanged(ref selectedItem, value, nameof(SelectedItem));
    }

    public ProjectEditingViewModel()
    {
        items = new();

        CloseItemCommand = ReactiveCommand.Create<FileProjectItemViewModel>((item) =>
        {
            if (item is not null)
            {
                Items.Remove(item);
            }
        });

        this.WhenActivated((CompositeDisposable disposables) =>
        {
            ProjectEditingInteractions
            .EditItem
            .RegisterHandler(interation =>
            {
                if (!Items.Contains(interation.Input))
                {
                    Items.Add(interation.Input);
                }
                interation.SetOutput(interation.Input);
            })
            .DisposeWith(disposables);

            this.WhenPropertyChanged(vm => vm.SelectedItem, true)
           .Where(prop => prop.Value is not null)
           .Subscribe(prop =>
           {
               Debug.WriteLine(prop?.Value?.Path);
           })

          .DisposeWith(disposables);
        });
    }
    public ReactiveCommand<FileProjectItemViewModel, Unit> CloseItemCommand
    {
        get; private set;
    }
}
