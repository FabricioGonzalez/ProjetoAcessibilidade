using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckboxItemViewModel : INotifyPropertyChanged
{
    public string Topic
    {
        get;
    }

    public IOptionsContainerViewModel Options
    {
        get;
    }

    public List<ITextFormItemViewModel> TextItems
    {
        get;
    }
}