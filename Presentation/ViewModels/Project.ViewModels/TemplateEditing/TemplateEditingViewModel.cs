using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

using App.Core.Entities.Solution.Explorer;

using AppViewModels.Common;
using AppViewModels.TemplateEditing.Models;

using Common;

using DynamicData.Binding;

using Project.Application.App.Queries.GetAllTemplates;
using Project.Application.Contracts;

using ReactiveUI;

using Splat;

namespace AppViewModels.TemplateEditing;
public class TemplateEditingViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen
    {
        get; set;
    }
    public string UrlPathSegment { get; } = "TemplateEditing";

    private ObservableCollectionExtended<ItemTemplateModel> projectItems;

    public ObservableCollectionExtended<ItemTemplateModel> ProjectItems
    {
        get => projectItems;
        set => this.RaiseAndSetIfChanged(ref projectItems, value);

    }

    private readonly IQueryDispatcher queryDispatcher;

    public TemplateEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();

        AddNewItemCommand = ReactiveCommand.Create(() =>
        {
            ProjectItems.Add(new() { IsEditing = true });
        });

        CommitItemCommand = ReactiveCommand.Create<ItemTemplateModel>((item) =>
        {
            var result = ProjectItems.FirstOrDefault(x => x == item);

            if (result is not null)
            {
                result.Path = Path.Combine(Constants.AppItemsTemplateFolder, $"{result.Name}{Constants.AppProjectTemplateExtension}");
            }

        });

        this.WhenActivated(async (CompositeDisposable disposables) =>
        {
            var result = await queryDispatcher.Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(new(), CancellationToken.None);

            result.OnError(out var data, out string message)
            .OnLoading(out data, out var isLoading)
            .OnSuccess(out data);

            if (data is not null)
            {
                ProjectItems = new(data.Select(item => new ItemTemplateModel()
                {
                    Name = item.Name,
                    Path = item.Path,
                    IsEditing = false
                }));
            }

        });
    }

    public ReactiveCommand<Unit, Unit> AddNewItemCommand
    {
        get;
    }
    public ReactiveCommand<ItemTemplateModel, Unit> CommitItemCommand
    {
        get;
    }
}
