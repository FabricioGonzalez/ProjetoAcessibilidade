using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IOptionsContainerViewModel : INotifyPropertyChanged
{
    public ObservableCollection<IOptionViewModel> Options
    {
        get;
    }
}