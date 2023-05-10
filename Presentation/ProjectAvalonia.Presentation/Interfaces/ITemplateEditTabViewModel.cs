using System.ComponentModel;
using ProjectAvalonia.Presentation.States;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface ITemplateEditTabViewModel : INotifyPropertyChanged
{
    public AppModelState EditingItem
    {
        get;
        set;
    }
}