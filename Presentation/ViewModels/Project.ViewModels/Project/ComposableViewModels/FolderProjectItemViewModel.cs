using System.Collections.ObjectModel;
using System.Reactive;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
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

    public string Title
    {
        get; set;
    }

    public bool InEditMode
    {
        get => _isInEditMode;
        set => this.RaiseAndSetIfChanged(ref _isInEditMode, value);
    }

    private ObservableCollection<ProjectItemViewModel> _children = new();
    public ObservableCollection<ProjectItemViewModel> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(ref _children, value);
    }

    public ReactiveCommand<Unit, Unit> RenameCommand
    {
        get;
    }
}
