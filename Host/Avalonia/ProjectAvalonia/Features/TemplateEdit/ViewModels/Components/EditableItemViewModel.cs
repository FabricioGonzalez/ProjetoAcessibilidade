using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;

public class EditableItemViewModel : ReactiveObject, IEditableItemViewModel
{
    public string Name
    {
        get;
        set;
    }

    public bool InEditMode
    {
        get;
        set;
    }

    public string TemplateName
    {
        get;
        set;
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

    public ReactiveCommand<Unit, Unit> CommitItemCommand
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
}