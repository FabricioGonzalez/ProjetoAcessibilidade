using System.Reactive;
using Avalonia.Input;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeEditingItemViewModel
    : ReactiveObject
        , IEditingItemViewModel
{
    public KeyGesture Gesture
    {
        get;
    } = new(Key.S, InputModifiers.Control);

    public IEditingBodyViewModel Body
    {
        get;
    } = new DesingTimeEditingBodyViewModel();

    public string TemplateName
    {
        get;
    }

    public string ItemName
    {
        get;
    } = "Teste";

    public string Id
    {
        get;
    }

    public bool IsSaved
    {
        get;
    } = true;

    public string ItemPath
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }
}