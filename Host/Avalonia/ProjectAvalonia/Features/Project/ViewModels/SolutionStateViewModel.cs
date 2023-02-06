using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using AppViewModels.Common;

using Avalonia.Threading;

using Core.Entities.App;
using Core.Entities.Solution.ItemsGroup;
using Core.Entities.Solution.ReportInfo;

using DynamicData.Binding;

using Project.Application.App.Queries.GetUFList;
using Project.Application.Contracts;

using ProjectAvalonia.Common.Helpers;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels;
public partial class SolutionStateViewModel : ViewModelBase
{
    [AutoNotify] private SolutionInfo _reportData = new();

    [AutoNotify] private string _fileName = "";

    [AutoNotify] private string _filePath = "";

    [AutoNotify] private string _parentFolderName = "";

    [AutoNotify] private string _parentFolderPath = "";


    [AutoNotify] private ObservableCollectionExtended<ItemGroupModel> _itemGroups = new();

    [AutoNotify] private ObservableCollectionExtended<UFModel> _ufList;

    private readonly IQueryDispatcher queryDispatcher;

    public SolutionStateViewModel()
    {
        queryDispatcher = Locator.Current.GetService<IQueryDispatcher>();

        ChooseSolutionPath = ReactiveCommand.Create(async () =>
        {
            var path = await FileDialogHelper.ShowOpenFolderDialogAsync("Local da Solução");

            Dispatcher.UIThread.Post(() =>
            {
                FilePath = path;
                FileName = Path.GetFileNameWithoutExtension(path);
            });
        });

        ChooseLogoPath = ReactiveCommand.Create(async () =>
        {
            var path = await FileDialogHelper.ShowOpenFileDialogAsync("Logo da Empresa", new string[] { "png" });

            Dispatcher.UIThread.Post(() =>
            {
                ReportData.LogoPath = path;
            });
        });

        Task.Run(async () =>
        {
            var result = new ObservableCollectionExtended<UFModel>(
           (await queryDispatcher
         .Dispatch<GetAllUFQuery, IList<UFModel>>(new(), CancellationToken.None)
         ).OrderBy(x => x.Name));

            Dispatcher.UIThread.Post(() => UfList = result);
        });
    }

    public ICommand ChooseSolutionPath
    {
        get;
    }
    public ICommand ChooseLogoPath
    {
        get;
    }
}
