using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ISolutionEditingBody : INotifyPropertyChanged
{
    public string Nome
    {
        get;
        set;
    }
}