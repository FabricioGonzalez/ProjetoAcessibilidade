using System.Collections.ObjectModel;

using AppViewModels.Interactions.Project;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public class FolderProjectItemViewModel : ProjectItemViewModel
{
    public FolderProjectItemViewModel(string title, string path, bool inEditMode = false)
        : base(Title: title, Path: path, inEditMode: inEditMode)
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });

        this.WhenPropertyChanged(vm => vm.Title, notifyOnInitialValue: false)
             .Subscribe(item =>
             {
                 if (item is not null && item.Value is not null)
                 {
                     ProjectInteractions
                    .RenameFolderInteraction
                    .Handle(this)
                    .Subscribe();
                 }
             });

        DeleteCommand.Subscribe(disposable =>
        {
            ProjectInteractions
            .DeleteFolderInteraction
            .Handle(this)
            .Subscribe();
        });

    }

    private ObservableCollectionExtended<ProjectItemViewModel> _children = new();
    public ObservableCollectionExtended<ProjectItemViewModel> Children
    {
        get => _children;
        set => this.RaiseAndSetIfChanged(ref _children, value);
    }
}
