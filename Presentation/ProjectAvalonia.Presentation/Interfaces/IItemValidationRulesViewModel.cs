using System.Collections.ObjectModel;
using System.ComponentModel;
using Core.Entities.ValidationRules;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IItemValidationRulesViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ValidationRule> ValidationRules
    {
        get;
        set;
    }

    public ObservableCollection<IValidationRuleContainerState> ValidationItemRules
    {
        get;
        set;
    }
}