using AppViewModels.Common;

using ReactiveUI;

namespace AppViewModels.System;
public class SettingsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "Settings";


}
