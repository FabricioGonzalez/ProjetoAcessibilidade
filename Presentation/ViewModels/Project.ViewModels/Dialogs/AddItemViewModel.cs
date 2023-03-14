using System.Reactive;

using AppViewModels.Common;

using Common;

using Core.Entities.Solution.Explorer;

using DynamicData.Binding;

using Project.Domain.App.Queries.GetAllTemplates;
using Project.Domain.Contracts;

using ReactiveUI;

using Splat;

namespace AppViewModels.Dialogs;
public class AddItemViewModel : ViewModelBase
{
    private ObservableCollectionExtended<ExplorerItem> items;
    public ObservableCollectionExtended<ExplorerItem> Items
    {
        get => items;
        set => this.RaiseAndSetIfChanged(ref items, value, nameof(Items));
    }

    private ExplorerItem item;
    public ExplorerItem Item
    {
        get => item;
        set => this.RaiseAndSetIfChanged(ref item, value, nameof(Item));
    }


    private readonly IQueryDispatcher queryDispatcher;

    public AddItemViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();


        this.WhenActivated(async (Action<IDisposable> disposables) =>
        {
            Items = new(await GetItems());
        });

        SelectItemToCreateCommand = ReactiveCommand.Create(() =>
        {
            if (Item is not null)
            {
                return Item;
            }
            return null;
        });
    }

    public async Task<List<ExplorerItem>> GetItems()
    {
        var result = await queryDispatcher.Dispatch<GetAllTemplatesQuery, Resource<List<ExplorerItem>>>(new(), CancellationToken.None);

        result
            .OnError(out var data, out var message)
            .OnLoading(out data, out var isLoading)
            .OnSuccess(out data);

        if (data is not null)
        {
            return data;
        }

        return new();

    }

    public ReactiveCommand<Unit, ExplorerItem?> SelectItemToCreateCommand
    {
        get; set;
    }
    public ReactiveCommand<Unit, Unit> CloseDialogCommand
    {
        get; set;
    }
}
