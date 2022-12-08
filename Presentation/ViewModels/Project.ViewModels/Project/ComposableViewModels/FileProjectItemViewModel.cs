using System;
using System.Reactive.Disposables;

using AppViewModels.Interactions.Project;

using ReactiveUI;

namespace AppViewModels.Project.ComposableViewModels;
public class FileProjectItemViewModel : ProjectItemViewModel
{
    public FileProjectItemViewModel()
    {
        RenameCommand = ReactiveCommand.Create(() =>
        {
            InEditMode = true;
        });

        DeleteCommand = ReactiveCommand.Create(() =>
        {

        });
        RenameCommand.Subscribe(disposable =>
        {
            ProjectInteractions
            .RenameFileInteraction
           .Handle(this)
            .Subscribe();
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
