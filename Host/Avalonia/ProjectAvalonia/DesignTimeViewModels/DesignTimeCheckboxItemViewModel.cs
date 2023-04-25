using System.Collections.ObjectModel;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeCheckboxItemViewModel : ReactiveObject, ICheckboxItemViewModel
{
    public string Topic
    {
        get;
    } = "Teste Checkbox";

    public IOptionsContainerViewModel Options
    {
        get;
    } = new DesignTimeOptionContainerViewModel();

    public ObservableCollection<ITextFormItemViewModel> TextItems
    {
        get;
    } = new()
    {
        new DesignTimeTextFormItem(),
        new DesignTimeTextFormItem()
    };
}