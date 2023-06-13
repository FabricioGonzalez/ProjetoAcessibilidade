using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Dialogs;

public class DeleteDialogViewModel
    : DialogViewModelBase<bool>
        , IDeleteDialogViewModel
{
    private string _title;

    public DeleteDialogViewModel(
        string message
        , string title
        , string caption
    )
    {
        Message = message;
        _title = title;
        Caption = caption;

        NextCommand = ReactiveCommand.Create(() => Close(result: true));

        CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel));

        SetupCancel(true, true, true);
    }

    public string Message
    {
        get;
    }

    public string Caption
    {
        get;
    }
    public override MenuViewModel? ToolBar => null;
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