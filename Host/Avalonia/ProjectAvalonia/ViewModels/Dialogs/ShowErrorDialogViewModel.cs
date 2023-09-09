using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.ViewModels.Dialogs;

public class ShowErrorDialogViewModel : DialogViewModelBase<bool>
{
    private string _title;

    public ShowErrorDialogViewModel(
        string message
        , string title
        , string caption
    )
    {
        Message = message;
        _title = title;
        Caption = caption;

        NextCommand = ReactiveCommand.Create(() => Close());

        SetupCancel(false, true, true);
    }
    public override MenuViewModel? ToolBar => null;
    public string Message
    {
        get;
    }

    public string Caption
    {
        get;
    }

    public override string Title
    {
        get => _title;
        protected set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;
}