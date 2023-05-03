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

        NextCommand = ReactiveCommand.Create(execute: () => Close());

        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);
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

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;
}