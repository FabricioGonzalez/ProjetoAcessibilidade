using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IProjectEditingViewModel : INotifyPropertyChanged
{
    public IEditingItemViewModel SelectedItem
    {
        get;
    }

    public ReadOnlyObservableCollection<IEditingItemViewModel> EditingItems
    {
        get;
    }

    public ReactiveCommand<IItemViewModel, Unit> AddItemToEdit
    {
        get;
    }
}