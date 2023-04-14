using System.Collections.Generic;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesingTimeEditingBodyViewModel : ReactiveObject, IEditingBodyViewModel
{
    public List<ILawListViewModel> LawList
    {
        get;
    } = new()
    {
        new DesignTimeLawListViewModel(),
        new DesignTimeLawListViewModel()
    };

    public List<IFormViewModel> Form
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