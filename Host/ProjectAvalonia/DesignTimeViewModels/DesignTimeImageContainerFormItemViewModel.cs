using System.Collections.ObjectModel;
using System.Reactive;

using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeImageContainerFormItemViewModel : ReactiveObject, IImageFormItemViewModel
{
    public string Observation
    {
        get;
    } =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Faucibus vitae aliquet nec ullamcorper sit. Velit dignissim sodales ut eu sem integer. Aliquam etiam erat velit scelerisque in dictum non consectetur a. Dignissim diam quis enim lobortis scelerisque fermentum. Integer enim neque volutpat ac tincidunt vitae. Tempor nec feugiat nisl pretium fusce id velit ut tortor. Id diam vel quam elementum. Neque gravida in fermentum et sollicitudin. Lobortis elementum nibh tellus molestie nunc non blandit.";

    public string Topic
    {
        get;
    } = "Imagens";
    public ReactiveCommand<IImageItemViewModel, Unit> RemoveImageCommand
    {
        get;
    }

    public ObservableCollection<IImageItemViewModel> ImageItems
    {
        get;
    } = new()
    {
        new DesignTimeImageViewModel(),
        new DesignTimeImageViewModel(),
        new DesignTimeImageViewModel(),
        new DesignTimeImageViewModel()
    };


    public ReactiveCommand<Unit, Unit> AddPhotoCommand
    {
        get;
    }
}