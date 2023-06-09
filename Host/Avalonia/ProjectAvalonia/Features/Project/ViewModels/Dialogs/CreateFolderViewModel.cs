using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Dialogs;

public partial class CreateFolderViewModel
    : DialogViewModelBase<string>
        , ICreateFolderViewModel
{
    [AutoNotify] private string _folderName = "";

    private string _title;

    public CreateFolderViewModel(
        string message
        , string title
        , string caption,
         ObservableCollection<IItemGroupViewModel> items
    )
    {
        Message = message;
        _title = title;
        Caption = caption;
        var canCreate = this.WhenAnyValue(vm => vm.FolderName)
            .Select(folder => !string.IsNullOrEmpty(folder) && !items.Any(x => x.Name == FolderName));

        NextCommand = ReactiveCommand.Create(() => Close(result: FolderName), canCreate);

        CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel, ""));

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