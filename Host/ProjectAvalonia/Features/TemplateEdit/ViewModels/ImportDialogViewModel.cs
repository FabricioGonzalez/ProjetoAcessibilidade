using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;

using ProjectAvalonia.Features.TemplateEdit.Services;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;

using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

public partial class ImportDialogViewModel : DialogViewModelBase<ProjectTemplateImportLocation>
{
    private string _title = "Importar";
    private string _localizedTitle;


    public IEnumerable<ProjectImport> ImportLocations
    {
        get; set;
    }

    private string _selectedFile;
    public string SelectedFile
    {
        get => _selectedFile;
        set => this.RaiseAndSetIfChanged(ref _selectedFile, value);
    }

    private ProjectImport _selectItem;
    private readonly ImportTemplateService _importTemplateService;
    private readonly IFilePickerService _filePickerService;

    public ProjectImport SelectedItem
    {
        get => _selectItem;
        set => this.RaiseAndSetIfChanged(ref _selectItem, value);
    }
    public override string Title
    {
        get => _title;
        protected set => this.RaiseAndSetIfChanged(ref _title, value);
    }
    public override string? LocalizedTitle
    {
        get => _localizedTitle;
        protected set => this.RaiseAndSetIfChanged(ref _localizedTitle, value);
    }
    public ImportDialogViewModel(ImportTemplateService importTemplateService, IFilePickerService filePickerService)
    {
        NextCommand = ReactiveCommand.Create(() => Close(result: SelectedItem.Location));

        CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel));

        ImportLocations = new List<ProjectImport>()
        {
            new()
            {
                Name = "Web / Github",
                Location = ProjectTemplateImportLocation.FromGithub
            },
             new()
            {
                Name = "Local",
                Location = ProjectTemplateImportLocation.FromFile
            },
        };



        SetupCancel(enableCancel: true, enableCancelOnEscape: false, enableCancelOnPressed: false);
        _importTemplateService = importTemplateService;
        _filePickerService = filePickerService;
    }

    public ReactiveCommand<Unit, Unit> SelectFileCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        if (await _filePickerService.GetFolderAsync() is { } file)
        {
            SelectedFile = file;
        }
    });

    public ReactiveCommand<Unit, Unit> ImportItemTemplateCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        if (SelectedItem.Location == ProjectTemplateImportLocation.FromFile)
        {

            await _importTemplateService.ImportTemplatesFromFile();
        }
        if (SelectedItem.Location == ProjectTemplateImportLocation.FromGithub)
        {

            await _importTemplateService.ImportTemplatesFromGithub(SelectedFile);
        }

    }, this.WhenAnyValue(vm => vm.SelectedItem, vm => vm.SelectedFile)
            .Select(it => it.Item1 is not null && (it.Item1.Name == "Local" || !string.IsNullOrWhiteSpace(it.Item2))));

    public override MenuViewModel? ToolBar => null;
}

public class ProjectImport
{
    public string Name
    {
        get; set;
    }

    public ProjectTemplateImportLocation Location
    {
        get; set;
    }
}