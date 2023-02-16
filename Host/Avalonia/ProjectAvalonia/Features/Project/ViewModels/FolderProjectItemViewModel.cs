using DynamicData.Binding;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;
public class FolderProjectItemViewModel : ProjectItemViewModel
{
    public FolderProjectItemViewModel(string title, string path, string referencedItem, bool inEditMode = false)
        : base(Title: title, Path: path, referencedItem: referencedItem, inEditMode: inEditMode)
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });

        CommitChangeCommand = ReactiveCommand.Create(() =>
        {

        });

    }

    private ObservableCollectionExtended<ProjectItemViewModel> _children = new();
    public ObservableCollectionExtended<ProjectItemViewModel> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(ref _children, value);
    }
}
