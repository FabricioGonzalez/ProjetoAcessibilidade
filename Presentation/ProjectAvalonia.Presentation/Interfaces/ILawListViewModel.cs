using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ILawListViewModel : INotifyPropertyChanged
{
    public string LawId
    {
        get;
    }

    public string LawContent
    {
        get;
    }
}