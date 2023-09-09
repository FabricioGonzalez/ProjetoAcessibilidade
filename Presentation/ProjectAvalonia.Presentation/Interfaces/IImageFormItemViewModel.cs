using System.Collections.ObjectModel;
using System.Reactive;

using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IImageFormItemViewModel : IFormViewModel
{
    public string Topic
    {
        get;
    }

    public ObservableCollection<IImageItemViewModel> ImageItems
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddPhotoCommand
    {
        get;
    }
    public ReactiveCommand<IImageItemViewModel, Unit> RemoveImageCommand
    {
        get;
    }
}