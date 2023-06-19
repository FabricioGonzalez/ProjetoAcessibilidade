using System.Collections.ObjectModel;
using System.Reactive;

using Avalonia.Input;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesingTimeEditingBodyViewModel : ReactiveObject, IEditingBodyViewModel
{
    public ObservableCollection<ILawListViewModel> LawList
    {
        get;
    } = new()
    {
        new DesignTimeLawListViewModel(),
        new DesignTimeLawListViewModel()
    };
    public KeyGesture Gesture
    {
        get;
    } = new KeyGesture(Key.S, KeyModifiers.Control);
    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }
    public ObservableCollection<IFormViewModel> Form
    {
        get;
    } = new()
    {
        new DesignTimeTextFormItem(),
        new DesignTimeCheckboxFormItem(),
        new DesignTimeCheckboxFormItem(),
        new DesignTimeCheckboxFormItem(),
        new DesignTimeImageContainerFormItemViewModel(),
        new DesignTimeObservationFormItem()
    };
}