using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Dialogs;

public class DeleteDialogViewModel : DialogViewModelBase<bool>
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

        NextCommand = ReactiveCommand.Create(execute: () => Close(result: true));

        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel, result: false));

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

    public override string Title
    {
        get => _title;
        protected set => this.RaiseAndSetIfChanged(backingField: ref _title, newValue: value);
    }
}