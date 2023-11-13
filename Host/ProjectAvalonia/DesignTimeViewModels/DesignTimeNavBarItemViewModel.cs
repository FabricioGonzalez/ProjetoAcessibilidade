using System;
using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeNavBarItemViewModel
    : ReactiveObject
        , INavBarItemViewModel
{
    public DesignTimeNavBarItemViewModel()
    {
        OpenCommand = ReactiveCommand.Create<bool>(obs => Console.WriteLine(obs));
    }

    public string LocalizedTitle
    {
        get;
        set;
    } = "Teste de sistema";

    public string Title
    {
        get;
        set;
    }

    public bool IsSelected
    {
        get;
        set;
    }

    public bool IsSelectable
    {
        get;
    }

    public ReactiveCommand<bool, Unit> OpenCommand
    {
        get;
    }

    public void Toggle()
    {
    }
}