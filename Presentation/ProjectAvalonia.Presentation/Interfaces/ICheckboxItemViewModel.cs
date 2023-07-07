using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

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

    public bool IsInvalid
    {
        get;
        set;
    }

    public IOptionsContainerViewModel Options
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> InvalidateItemCommand => ReactiveCommand.Create(() =>
    {
        IsInvalid = !IsInvalid;
    });

    public ObservableCollection<ITextFormItemViewModel> TextItems
    {
        get;
    }
}