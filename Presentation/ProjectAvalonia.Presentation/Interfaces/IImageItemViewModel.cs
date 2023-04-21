using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IImageItemViewModel : INotifyPropertyChanged
{
    public string ImagePath
    {
        get;
    }

    public string ImageObservation
    {
        get;
    }
}