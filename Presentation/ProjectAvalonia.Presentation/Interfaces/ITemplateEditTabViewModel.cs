using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ProjectAvalonia.Presentation.States;
using ProjectAvalonia.Presentation.States.FormItemState;
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

    public ReactiveCommand<FormItemContainer, Unit> RemoveItemCommand
    {
        get;
    }

    public ReactiveCommand<LawStateItem, Unit> RemoveLawCommand
    {
        get;
    }

    public ReactiveCommand<string, Unit> AddRuleCommand
    {
        get;
    }

    public ReactiveCommand<(string, IValidationRuleState), Unit> EditRuleCommand
    {
        get;
    }
}