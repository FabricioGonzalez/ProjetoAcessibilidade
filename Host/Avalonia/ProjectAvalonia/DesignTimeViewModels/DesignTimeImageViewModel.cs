using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.DesignTimeViewModels;

public class DesignTimeImageViewModel : ReactiveObject, IImageItemViewModel
{
    public string ImagePath
    {
        get;
    } = @"C:\Users\Ti\Pictures\Captura de tela 2023-03-20 121203.png";

    public string ImageObservation
    {
        get;
    } =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Faucibus vitae aliquet nec ullamcorper sit. Velit dignissim sodales ut eu sem integer. Aliquam etiam erat velit scelerisque in dictum non consectetur a. Dignissim diam quis enim lobortis scelerisque fermentum. Integer enim neque volutpat ac tincidunt vitae. Tempor nec feugiat nisl pretium fusce id velit ut tortor. Id diam vel quam elementum. Neque gravida in fermentum et sollicitudin. Lobortis elementum nibh tellus molestie nunc non blandit.";
}