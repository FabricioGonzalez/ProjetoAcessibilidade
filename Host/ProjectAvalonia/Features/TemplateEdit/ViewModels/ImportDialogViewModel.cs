using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ProjectAvalonia.Features.TemplateEdit.Services;
using ProjectAvalonia.Presentation.Interfaces.Services;
using ProjectAvalonia.ViewModels;
using ProjectAvalonia.ViewModels.Dialogs.Base;
using ReactiveUI;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

public class ImportDialogViewModel : DialogViewModelBase<ProjectTemplateImportLocation>
{
    private readonly IFilePickerService _filePickerService;
    private readonly ImportTemplateService _importTemplateService;
    private string _localizedTitle;

    private bool _overwriteContent = true;

    private string _selectedFile;

    private ProjectImport _selectItem;
    private string _title = "Importar";

    public ImportDialogViewModel(
        ImportTemplateService importTemplateService
        , IFilePickerService filePickerService
    )
    {
        NextCommand = ReactiveCommand.Create(() => Close(result: SelectedItem.Location));

        CancelCommand = ReactiveCommand.Create(() => Close(DialogResultKind.Cancel));

        ImportLocations = new List<ProjectImport>
        {
            new()
            {
                Name = "Web / Github", Location = ProjectTemplateImportLocation.FromGithub
            }
            , new()
            {
                Name = "Local", Location = ProjectTemplateImportLocation.FromFile
            }
        };


        SetupCancel(enableCancel: true, enableCancelOnEscape: false, enableCancelOnPressed: false);
        _importTemplateService = importTemplateService;
        _filePickerService = filePickerService;
    }


    public IEnumerable<ProjectImport> ImportLocations
    {
        get;
        set;
    }

    public bool OverwriteContent
    {
        get => _overwriteContent;
        set => this.RaiseAndSetIfChanged(backingField: ref _overwriteContent, newValue: value);
    }

    public string SelectedFile
    {
        get => _selectedFile;
        set => this.RaiseAndSetIfChanged(backingField: ref _selectedFile, newValue: value);
    }

    public ProjectImport SelectedItem
    {
        get => _selectItem;
        set => this.RaiseAndSetIfChanged(backingField: ref _selectItem, newValue: value);
    }

    public override string Title
    {
        get => _title;
        protected set => this.RaiseAndSetIfChanged(backingField: ref _title, newValue: value);
    }

    public override string? LocalizedTitle
    {
        get => _localizedTitle;
        protected set => this.RaiseAndSetIfChanged(backingField: ref _localizedTitle, newValue: value);
    }

    public ReactiveCommand<Unit, Unit> SelectFileCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        if (await _filePickerService.GetFolderAsync() is { } file)
        {
            SelectedFile = file;
        }
    });

    public ReactiveCommand<Unit, Unit> ImportItemTemplateCommand => ReactiveCommand.CreateFromTask(execute: async () =>
    {
        if (SelectedItem.Location == ProjectTemplateImportLocation.FromFile)
        {
            await _importTemplateService.ImportTemplatesFromFile(OverwriteContent);
        }

        if (SelectedItem.Location == ProjectTemplateImportLocation.FromGithub)
        {
            await _importTemplateService.ImportTemplatesFromGithub(selectedFile: SelectedFile
                , overwriteContent: OverwriteContent);
        }
    }, canExecute: this.WhenAnyValue(property1: vm => vm.SelectedItem, property2: vm => vm.SelectedFile)
        .Select(it => it.Item1 is not null && (it.Item1.Name == "Local" || !string.IsNullOrWhiteSpace(it.Item2))));

    public override MenuViewModel? ToolBar => null;
}

public class ProjectImport
{
    public string Name
    {
        get;
        set;
    }

    public ProjectTemplateImportLocation Location
    {
        get;
        set;
    }
}