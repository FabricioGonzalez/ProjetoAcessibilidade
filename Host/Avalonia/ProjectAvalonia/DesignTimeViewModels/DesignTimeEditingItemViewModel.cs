using System.Reactive;

using Avalonia.Input;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeEditingItemViewModel : ReactiveObject, IEditingItemViewModel
{
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
    public KeyGesture Gesture
    {
        get;
    } = new KeyGesture(Key.S, InputModifiers.Control);
    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }
    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }

    public IEditingBodyViewModel Body
    {
        get;
    } = new DesingTimeEditingBodyViewModel();
}