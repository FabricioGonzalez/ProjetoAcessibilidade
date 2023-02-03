using System;
using System.Reactive.Linq;

using ProjectAvalonia.Common.Models.FileItems;
using ProjectAvalonia.Logging;

using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Template Editing",
    Caption = "Manage general settings",
    Order = 0,
    Searchable = false,
    Category = "Templates",
    Keywords = new[]
    {
            "Settings", "General", "Bitcoin", "Dark", "Mode", "Run", "Computer", "System", "Start", "Background", "Close",
            "Auto", "Copy", "Paste", "Addresses", "Custom", "Change", "Address", "Fee", "Display", "Format", "BTC", "sats"
    },
    IconName = "settings_general_regular")]

public partial class TemplateEditTabViewModel : TemplateEditTabViewModelBase
{
    [AutoNotify] private FileItem _selectedItem;


    public TemplateEditTabViewModel()
    {
        this.WhenAnyValue(vm => vm.SelectedItem)
            .WhereNotNull()
            .Subscribe((prop) =>
            {
                Logger.LogDebug(prop.Name);
            });
    }

}
