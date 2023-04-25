using System.Collections.ObjectModel;
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