using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Core.ViewModels.Extensions;
using ReactiveUI;

namespace Project.Core.ViewModels.Project;
public class AddItemViewModel : ViewModelBase
{
    private bool _isBusy;
    private string? _searchText;

    public string? SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }
}
