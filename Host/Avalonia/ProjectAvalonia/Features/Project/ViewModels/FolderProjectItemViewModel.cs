using DynamicData.Binding;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class FolderProjectItemViewModel : ProjectItemViewModel
{
    private ObservableCollectionExtended<ProjectItemViewModel> _children = new();

    public FolderProjectItemViewModel(
        string title
        , string path
        , string referencedItem
        , bool inEditMode = false
    )
        : base(Title: title, Path: path, referencedItem: referencedItem, inEditMode: inEditMode)
    {
        RenameCommand = ReactiveCommand.Create(execute: () =>
        {
            InEditMode = true;
        });

        DeleteCommand = ReactiveCommand.Create(execute: () =>
        {
        });

        CommitChangeCommand = ReactiveCommand.Create(execute: () =>
        {
        });
    }

    public ObservableCollectionExtended<ProjectItemViewModel> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(backingField: ref _children, newValue: value);
    }
}