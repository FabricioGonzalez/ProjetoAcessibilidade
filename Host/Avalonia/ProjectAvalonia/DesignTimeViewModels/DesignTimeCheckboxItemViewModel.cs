using System.Collections.Generic;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeCheckboxItemViewModel : ReactiveObject, ICheckboxItemViewModel
{
    public string Topic
    {
        get;
    }

    public IOptionsContainerViewModel Options
    {
        get;
    } = new DesignTimeOptionContainerViewModel();

    public List<ITextFormItemViewModel> TextItems
    {
        get;
    } = new()
    {
        new DesignTimeTextFormItem(),
        new DesignTimeTextFormItem()
    };
}