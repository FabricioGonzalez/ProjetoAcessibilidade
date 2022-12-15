using System.Reactive;

using AppUsecases.App.Models;
using AppUsecases.App.Usecases;
using AppUsecases.Project.Entities.Project;

using AppViewModels.Common;
using AppViewModels.Contracts;

using DynamicData.Binding;

using ReactiveUI;

using Splat;

namespace AppViewModels.Dialogs.States;
public class SolutionStateViewModel : ViewModelBase
{
    private ReportDataModel reportData = new();
    public ReportDataModel ReportData
    {
        get => reportData;
        set => this.RaiseAndSetIfChanged(ref reportData, value, nameof(ReportData));
    }

    private string fileName = "";
    public string FileName
    {
        get => fileName;
        set => this.RaiseAndSetIfChanged(ref fileName, value, nameof(FileName));
    }

    private string filePath = "";
    public string FilePath
    {
        get => filePath;
        set => this.RaiseAndSetIfChanged(ref filePath, value, nameof(FilePath));
    }

    private string parentFolderName = "";
    public string ParentFolderName
    {
        get => parentFolderName;
        set => this.RaiseAndSetIfChanged(ref parentFolderName, value, nameof(ParentFolderName));
    }

    private string parentFolderPath = "";
    public string ParentFolderPath
    {
        get => parentFolderPath;
        set => this.RaiseAndSetIfChanged(ref parentFolderPath, value, nameof(ParentFolderPath));
    }

    private ObservableCollectionExtended<ItemGroupModel> itemGroups = new();
    public ObservableCollectionExtended<ItemGroupModel> ItemGroups
    {
        get => itemGroups;
        set => this.RaiseAndSetIfChanged(ref itemGroups, value, nameof(ItemGroups));
    }

    private ObservableCollectionExtended<UF> ufList;
    public ObservableCollectionExtended<UF> UFList
    {
        get => ufList;
        set => this.RaiseAndSetIfChanged(ref ufList, value, nameof(UFList));
    }
   
    private readonly GetUFList getUFList;
    private readonly IFileDialog dialogService;


    public SolutionStateViewModel()
    {
        getUFList = Locator.Current.GetService<GetUFList>();

        dialogService = Locator.Current.GetService<IFileDialog>();

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await dialogService.GetFolder();

            return path;
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await dialogService.GetFolder();

            return path;
        });

        UFList = new(getUFList
         .GetAllUF()
         .OrderBy(x => x.Name));
    }


    public ReactiveCommand<Unit, string> ChooseSolutionPath
    {
        get; set;
    }
    public ReactiveCommand<Unit, string> ChooseLogoPath
    {
        get; set;
    }
}
