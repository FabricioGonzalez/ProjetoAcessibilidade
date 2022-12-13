using System;
using System.Reactive.Disposables;

using AppViewModels.Interactions.Project;

using DynamicData.Binding;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public class FileProjectItemViewModel : ProjectItemViewModel
{
    public FileProjectItemViewModel(string title, string path, bool inEditMode = false) : base(title, path, inEditMode)
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });

        this.WhenPropertyChanged(vm => vm.Title)
            .Subscribe(item =>
            {
                if (item.Value != null)
                {
                    ProjectInteractions
                    .RenameFileInteraction
                    .Handle(this)
                    .Subscribe();
                }
            });

        DeleteCommand.Subscribe(disposable =>
        {
            ProjectInteractions
            .DeleteFileInteraction
            .Handle(this)
            .Subscribe();
        });
    }
}
