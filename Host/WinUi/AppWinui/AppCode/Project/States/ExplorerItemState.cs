using AppUsecases.Entities.FileTemplate;

using ReactiveUI;

namespace AppWinui.AppCode.Project.States;
public class ExplorerItemState : ReactiveObject
{
    private List<ExplorerItem>? _items;
    public List<ExplorerItem>? Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(
            ref _items,
            value,
            nameof(Items));
    }
    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(
            ref _errorMessage,
            value,
            nameof(ErrorMessage));
    }
    private bool? _isNotLoading = false;
    public bool? IsNotLoading
    {
        get => _isNotLoading;
        set => this.RaiseAndSetIfChanged(
            ref _isNotLoading,
            value,
            nameof(IsNotLoading));
    }
}
