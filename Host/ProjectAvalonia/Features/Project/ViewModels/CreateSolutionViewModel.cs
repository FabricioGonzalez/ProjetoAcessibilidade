﻿using System.Reactive.Linq;

using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels;

public class CreateSolutionViewModel : DialogViewModelBase<(string local, SolutionState solution)>
{
    private readonly IFilePickerService _fileDialogService;

    public CreateSolutionViewModel(
        string title
        , ILocationService _locationService,
        IFilePickerService fileDialogService
        , string caption = ""
    )
    {
        Title = title;
        _fileDialogService = fileDialogService;
        SolutionModel = new SolutionState(_locationService, _fileDialogService);

        Title = title;
        Caption = caption;

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: false);
        EnableBack = false;

        var backCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var nextCommandCanExecute = this
            .WhenAnyValue(vm => vm.SolutionModel.FileName,
            vm => vm.SolutionModel.FilePath)
            .Select(vals =>
            !string.IsNullOrWhiteSpace(vals.Item1)
            && !string.IsNullOrWhiteSpace(vals.Item2))
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        var cancelCommandCanExecute = this
            .WhenAnyValue(property1: x => x.IsDialogOpen)
            .ObserveOn(scheduler: RxApp.MainThreadScheduler);

        BackCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Back)
            , canExecute: backCommandCanExecute);
        NextCommand = ReactiveCommand.Create(execute: () =>
            {
                Close(result: (SolutionModel.FilePath, SolutionModel));
            }
            , canExecute: nextCommandCanExecute);
        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel)
            , canExecute: cancelCommandCanExecute);
    }

    public SolutionState SolutionModel
    {
        get;
    }

    public override MenuViewModel? ToolBar => null;


    public string Caption
    {
        get;
    }

    public override string Title
    {
        get;
        protected set;
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    }
}