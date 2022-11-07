using System.Reactive;

using Project.Core.ViewModels;

using ReactiveUI;

namespace Project.Core.ComposableViewModel;
public class FolderProjectItemViewModel : ProjectItemViewModel
{
    public FolderProjectItemViewModel()
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });
    }

    private bool _isInEditMode;

    public  string Title
    {
        get; set;
    }

    public bool InEditMode
    {
        get => _isInEditMode;
        set => this.RaiseAndSetIfChanged(ref _isInEditMode, value);
    }

    private List<ProjectItemViewModel> _children; 
    public List<ProjectItemViewModel> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(ref _children, value);
    }

    public ReactiveCommand<Unit, Unit> RenameCommand
    {
        get;
    }
}
