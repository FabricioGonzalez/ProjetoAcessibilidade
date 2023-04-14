using System.Reactive;
using ReactiveUI;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IImageFormItemViewModel : IFormViewModel
{
    public string Topic
    {
        get;
    }

    public List<IImageItemViewModel> ImageItems
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddPhotoCommand
    {
        get;
    }
}