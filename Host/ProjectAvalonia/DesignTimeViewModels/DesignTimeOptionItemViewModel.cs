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
        set;
    } = true;
    public string Id
    {
        get;
        set;
    }
    public bool ShouldBeUnchecked
    {
        get;
        set;
    }
    public bool IsInvalid
    {
        get;
        set;
    }
}

public class DesignTimeOptionUnCheckedItemViewModel : ReactiveObject, IOptionViewModel
{
    public string Value
    {
        get;
    } = "Não";
    public bool IsInvalid
    {
        get;
        set;
    }
    public bool IsChecked
    {
        get; set;
    } = false;
    public string Id
    {
        get;
        set;
    }
    public bool ShouldBeUnchecked
    {
        get;
        set;
    }
}