using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IItemValidationRulesViewModel : INotifyPropertyChanged
{
    public ObservableCollection<IValidationRuleContainerState> ValidationItemRules
    {
        get;
        set;
    }
}