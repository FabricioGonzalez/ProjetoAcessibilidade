using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITemplateEditViewModel : INotifyPropertyChanged
{
    public ObservableCollection<IEditableItemViewModel>? Items
    {
        get;
        set;
    }

    public IEditableItemViewModel? SelectedItem
    {
        get;
        set;
    }

    public ITemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddNewItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> LoadAllItems
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CommitItemCommand
    {
        get;
    }
}