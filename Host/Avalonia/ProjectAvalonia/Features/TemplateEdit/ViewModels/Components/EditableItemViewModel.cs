using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;

public partial class EditableItemViewModel
    : ReactiveObject
        , IEditableItemViewModel
{
    [AutoNotify] private bool _inEditMode;
    [AutoNotify] private string _name;


    [AutoNotify] private string _templateName;

    public EditableItemViewModel(
        ReactiveCommand<IEditableItemViewModel, Unit> commit
    )
    {
        CommitItemCommand = commit;
    }

    public string ItemPath
    {
        get;
        set;
    }

    public string Id
    {
        get;
        set;
    }

    public ReactiveCommand<IEditableItemViewModel, Unit> CommitItemCommand
    {
        get;
        init;
    }

    public ReactiveCommand<Unit, Unit> ExcludeItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameItemCommand
    {
        get;
    }
}