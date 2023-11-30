using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Dialogs;

public class DeleteDialogViewModel
    : DialogViewModelBase<(bool deleteForever, bool ok)>
        , IDeleteDialogViewModel
{
    private bool _deleteForever;
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

        NextCommand = ReactiveCommand.Create(() => Close(result: (DeleteForever, true)));

        CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel));

        SetupCancel(enableCancel: true, enableCancelOnEscape: true, enableCancelOnPressed: true);
    }

    public string Message
    {
        get;
    }

    public string Caption
    {
        get;
    }

    public bool DeleteForever
    {
        get => _deleteForever;
        set => this.RaiseAndSetIfChanged(backingField: ref _deleteForever, newValue: value);
    }

    public override MenuViewModel? ToolBar => null;

    public override string Title
    {
        get => _title;
        protected set => this.RaiseAndSetIfChanged(backingField: ref _title, newValue: value);
    }

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;
}