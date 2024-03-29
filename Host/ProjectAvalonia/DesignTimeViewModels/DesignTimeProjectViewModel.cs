﻿using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeProjectViewModel : ReactiveObject,
    IProjectViewModel
{
    public ReactiveCommand<Unit, Unit> SaveSolutionCommand
    {
        get;
    }

    public bool IsSolutionOpen
    {
        get;
    } = true;

    public IProjectExplorerViewModel ProjectExplorerViewModel
    {
        get;
    } = new DesignTimeProjectExplorerViewModel();

    public IProjectEditingViewModel ProjectEditingViewModel
    {
        get;
    } = new DesignTimeProjectEditingViewModel();

    public ReactiveCommand<Unit, Unit> OpenProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CreateProjectCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> PrintProjectCommand
    {
        get;
    }
}