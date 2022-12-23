using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

using AppViewModels.Common;
using AppViewModels.Interactions.Project;
using AppViewModels.Project.ComposableViewModels;

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
        });
    }
    public ReactiveCommand<FileProjectItemViewModel, Unit> CloseItemCommand
    {
        get; private set;
    }
}
