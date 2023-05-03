using System.Reactive.Linq;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Dialogs;

public partial class CreateFolderViewModel : DialogViewModelBase<string>, ICreateFolderViewModel
{
    [AutoNotify] private string _folderName = "";

    private string _title;

    public CreateFolderViewModel(
        string message
        , string title
        , string caption
    )
    {
        Message = message;
        _title = title;
        Caption = caption;
        var canCreate = this.WhenAnyValue(property1: vm => vm.FolderName)
            .Select(selector: folder => !string.IsNullOrEmpty(value: folder));


        NextCommand = ReactiveCommand.Create(execute: () => Close(result: FolderName), canExecute: canCreate);

        CancelCommand = ReactiveCommand.Create(execute: () => Close(kind: DialogResultKind.Cancel, result: ""));

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

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;
}