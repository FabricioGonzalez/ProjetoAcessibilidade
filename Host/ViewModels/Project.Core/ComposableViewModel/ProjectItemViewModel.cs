﻿using System.Reactive;

using Project.Core.ViewModels;

using ReactiveUI;

namespace Project.Core.ComposableViewModel;
public abstract class ProjectItemViewModel : ViewModelBase
{
    public ProjectItemViewModel()
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

    public ReactiveCommand<Unit, Unit> RenameCommand
    {
        get;
    }
}