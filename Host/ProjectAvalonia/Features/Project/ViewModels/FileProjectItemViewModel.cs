using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class FileProjectItemViewModel : ProjectItemViewModel
{
    public FileProjectItemViewModel(
        string title
        , string path
        , string referencedItem
        , bool inEditMode = false
    )
        : base(Title: title,
            Path: path,
            referencedItem: referencedItem,
            inEditMode: inEditMode)
    {
        RenameCommand = ReactiveCommand.Create(execute: () =>
        {
            InEditMode = true;
        });

        CommitChangeCommand = ReactiveCommand.Create(execute: () =>
        {
        });

        DeleteCommand = ReactiveCommand.Create(execute: () =>
        {
        });
    }
}