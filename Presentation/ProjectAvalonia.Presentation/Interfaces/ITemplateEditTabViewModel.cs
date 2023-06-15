using System.ComponentModel;
using System.Reactive;

using ProjectAvalonia.Presentation.States;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITemplateEditTabViewModel : INotifyPropertyChanged
{
    public AppModelState EditingItem
    {
        get;
        set;
    }
    public ReactiveCommand<Unit, Unit> AddItemCommand
    {
        get;
    }
    public ReactiveCommand<Unit, Unit> AddLawCommand
    {
        get;
    }
}