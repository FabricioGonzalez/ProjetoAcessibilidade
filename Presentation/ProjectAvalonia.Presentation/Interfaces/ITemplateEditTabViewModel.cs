using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.LawItemState;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITemplateEditTabViewModel : INotifyPropertyChanged
{
    public AppModelState EditingItem
    {
        get;
        set;
    }

    public ObservableCollection<IValidationRuleContainerState> EditingItemRules
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

    public ReactiveCommand<LawStateItem, Unit> RemoveLawCommand
    {
        get;
    }
}