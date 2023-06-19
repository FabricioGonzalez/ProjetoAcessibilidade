using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ICheckboxItemViewModel : INotifyPropertyChanged
{
    public string Topic
    {
        get;
    }
    public string Id
    {
        get;
        set;
    }
    public IOptionsContainerViewModel Options
    {
        get;
    }

    public ObservableCollection<ITextFormItemViewModel> TextItems
    {
        get;
    }
}