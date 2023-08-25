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
    } = new(key: Key.S, modifiers: KeyModifiers.Control);

    public string DisplayName
    {
        get;
    }

    public string TemplateName
    {
        get;
    }

    public IEditingBody Body
    {
        get;
        set;
    } = new DesingTimeEditingBodyViewModel();

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