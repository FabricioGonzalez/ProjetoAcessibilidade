using System.Collections.ObjectModel;
using System.Reactive;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeTemplateEditViewModel
    : ReactiveObject
        , ITemplateEditViewModel
{
    public ObservableCollection<IEditableItemViewModel> Items
    {
        get;
        set;
    } = new()
    {
        new DesignTimeEditableItemViewModel()
        , new DesignTimeEditableItemViewModel()
        , new DesignTimeEditableItemViewModel()
        , new DesignTimeEditableItemViewModel()
        , new DesignTimeEditableItemViewModel()
    };

    public IEditableItemViewModel SelectedItem
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

    public ReactiveCommand<IEditableItemViewModel, Unit> CommitItemCommand
    {
        get;
    }
}