using System.Collections.Immutable;

using ProjectAvalonia.Models;

using ReactiveUI;

namespace ProjectAvalonia.ViewModels;

public class MenuViewModel : ReactiveObject
{
    private bool _isVisible = true;

    public IImmutableList<IMenuItem> Items
    {
        get;
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => this.RaiseAndSetIfChanged(ref _isVisible, value);
    }

    public MenuViewModel(IImmutableList<IMenuItem> items)
    {
        Items = items;
    }
}
