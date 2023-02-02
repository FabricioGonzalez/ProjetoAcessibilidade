using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

using App.Core.Entities.App;
using App.Core.Entities.Solution.ItemsGroup;
using App.Core.Entities.Solution.ReportInfo;

using AppViewModels.Common;
using AppViewModels.Contracts;

using Avalonia.Threading;

using DynamicData.Binding;

using Project.Application.App.Queries.GetUFList;
using Project.Application.Contracts;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;
public class SolutionStateViewModel : ViewModelBase
{
    private SolutionInfo reportData = new();
    public SolutionInfo ReportData
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

    private ObservableCollectionExtended<UFModel> ufList;
    public ObservableCollectionExtended<UFModel> UFList
    {
        get => ufList;
        set => this.RaiseAndSetIfChanged(ref ufList, value, nameof(UFList));
    }

    private readonly IQueryDispatcher queryDispatcher;
    private readonly IFileDialog dialogService;

    public SolutionStateViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();

        dialogService = Locator.Current.GetService<IFileDialog>();

        ChooseSolutionPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await dialogService.GetFolder();

            return path;
        });

        ChooseLogoPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var path = await dialogService.GetFile(new string[] { "png" });

            return path;
        });

        Task.Run(async () =>
        {
            var result = new ObservableCollectionExtended<UFModel>(
           (await queryDispatcher
         .Dispatch<GetAllUFQuery, IList<UFModel>>(new(), CancellationToken.None)
         ).OrderBy(x => x.Name));

            Dispatcher.UIThread.Post(() => UFList = result);
        });
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
