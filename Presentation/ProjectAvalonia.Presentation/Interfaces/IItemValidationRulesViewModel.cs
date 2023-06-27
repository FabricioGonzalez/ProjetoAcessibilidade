using System.Collections.ObjectModel;

using Core.Entities.ValidationRules;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IItemValidationRulesViewModel
{
    public ObservableCollection<ValidationRule> ValidationRules
    {
        get; set;
    }
}