using System.Collections.ObjectModel;

namespace ProjectAvalonia.Presentation.Interfaces;

public interface IOptionsContainerViewModel
{
    public ObservableCollection<IOptionViewModel> Options
    {
        get;
    }
}