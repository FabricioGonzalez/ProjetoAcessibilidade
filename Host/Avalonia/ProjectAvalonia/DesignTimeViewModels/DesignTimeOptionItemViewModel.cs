using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeOptionCheckedItemViewModel : ReactiveObject, IOptionViewModel
{
    public string Value
    {
        get;
    } = "Sim";

    public bool IsChecked
    {
        get;
    } = true;
}

public class DesignTimeOptionUnCheckedItemViewModel : ReactiveObject, IOptionViewModel
{
    public string Value
    {
        get;
    } = "Não";

    public bool IsChecked
    {
        get;
    } = false;
}