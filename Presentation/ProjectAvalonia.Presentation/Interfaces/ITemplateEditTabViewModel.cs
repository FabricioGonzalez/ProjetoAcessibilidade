using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITemplateEditTabViewModel : INotifyPropertyChanged
{
    public ReactiveCommand<IEditableItemViewModel, Unit> LoadEditingItem
    {
        get;
    }
}