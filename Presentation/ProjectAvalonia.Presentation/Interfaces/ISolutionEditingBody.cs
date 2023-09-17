using System.ComponentModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ISolutionEditingBody
    : IEditingBody
{
}

public interface IEditingBody : INotifyPropertyChanged,IDisposable
{
}