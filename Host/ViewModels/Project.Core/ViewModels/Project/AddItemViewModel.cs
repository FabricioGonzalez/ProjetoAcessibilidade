using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.InteropServices;

using AppUsecases.Contracts.Usecases;
using AppUsecases.Editing.Entities;

using Common;

using DynamicData.Binding;

using Project.Core.ViewModels.Extensions;

using ReactiveUI;

using Splat;

namespace Project.Core.ViewModels.Project;
public class AddItemViewModel : ViewModelBase
{
    private bool _isBusy;

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    private ObservableCollectionExtended<FileTemplate> items;
    public ObservableCollectionExtended<FileTemplate> Items
    {
        get => items;
        set => this.RaiseAndSetIfChanged(ref items, value, nameof(Items));
    }

    private FileTemplate item;
    public FileTemplate Item
    {
        get => item;
        set => this.RaiseAndSetIfChanged(ref item, value, nameof(Item));
    }


    private readonly IQueryUsecase<List<FileTemplate>> readProjectItems;

    public AddItemViewModel()
    {
        readProjectItems ??= Locator.Current.GetService<IQueryUsecase<List<FileTemplate>>>();

        this.WhenActivated(async (Action<IDisposable> disposables) =>
        {
            Items = new(await GetItems());
        });

        SelectItemToCreateCommand = ReactiveCommand.Create(() =>
        {
            if (Item is not null)
            {
                Debug.WriteLine(Item.Name);

                return Item;
            }
            return null;
        });
    }

    public async Task<List<FileTemplate>> GetItems()
    {
        var result = await readProjectItems.executeAsync();

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

    public ReactiveCommand<Unit, FileTemplate?> SelectItemToCreateCommand
    {
        get; set;
    }

}
