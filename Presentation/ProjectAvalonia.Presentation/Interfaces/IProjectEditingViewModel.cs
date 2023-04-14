using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectEditingViewModel : INotifyPropertyChanged
{
    public IEditingItemViewModel SelectedItem
    {
        get;
    }

    public List<IEditingItemViewModel> EditingItems
    {
        get;
    }
}