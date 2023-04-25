using System.Reactive;
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

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }

    public IEditingBodyViewModel Body
    {
        get;
    } = new DesingTimeEditingBodyViewModel();
}