using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;

namespace AppViewModels.Common;
public class ViewModelBase : ReactiveObject, IActivatableViewModel, IValidatableViewModel
{
    public ViewModelActivator Activator { get; } = new ViewModelActivator();

    public ValidationContext ValidationContext { get; } = new ValidationContext();

    protected ObservableAsPropertyHelper<bool> isBusy;
    public bool IsBusy => isBusy.Value;
}
