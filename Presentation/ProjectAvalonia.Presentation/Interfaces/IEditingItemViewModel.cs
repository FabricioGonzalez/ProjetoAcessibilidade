using System.Reactive;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingItemViewModel
{
    public string ItemName
    {
        get;
    }

    public string Id
    {
        get;
    }

    public bool IsSaved
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }   
    
    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }


    public IEditingBodyViewModel Body
    {
        get;
    }
}