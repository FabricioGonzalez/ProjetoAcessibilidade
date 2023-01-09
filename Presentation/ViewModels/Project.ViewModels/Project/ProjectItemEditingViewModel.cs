using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;

using App.Core.Entities.Solution.Project.AppItem;

using AppViewModels.Common;
using AppViewModels.Project.ComposableViewModels;

using Common;

using Project.Application.Contracts;
using Project.Application.Project.Commands.ProjectItemCommands.SaveCommands;
using Project.Application.Project.Queries.GetProjectItemContent;

using ReactiveUI;

using Splat;

namespace AppViewModels.Project;
public class ProjectItemEditingViewModel : ViewModelBase
{
    private AppItemModel item;
    public AppItemModel Item
    {
        get => item;
        set => this.RaiseAndSetIfChanged(ref item, value, nameof(Item));
    }

    private FileProjectItemViewModel selectedItem;
    public FileProjectItemViewModel SelectedItem
    {
        get => selectedItem;
        set => this.RaiseAndSetIfChanged(ref selectedItem, value, nameof(SelectedItem));
    }

    private readonly IQueryDispatcher queryDispatcher;
    private readonly ICommandDispatcher commandDispatcher;

    public ProjectItemEditingViewModel()
    {
        queryDispatcher ??= Locator.Current.GetService<IQueryDispatcher>();
        commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        SaveItemCommand = ReactiveCommand.CreateFromTask<AppItemModel>(async (param) =>
        {
            Debug.WriteLine("Hotkey funcionando", param.ItemName);
            await commandDispatcher
            .Dispatch<SaveProjectItemContentCommand, Resource<object>>(
                new(param, SelectedItem.Path),
                CancellationToken.None);
        });

        this.WhenActivated((CompositeDisposable disposables) =>
        {
        });
    }

    public ReactiveCommand<AppItemModel, Unit> SaveItemCommand
    {
        get; private set;
    }
    public async Task SetEditingItem(string path)
    {
        var result = await queryDispatcher.Dispatch<GetProjectItemContentQuery, Resource<AppItemModel>>(new(path), CancellationToken.None);
        if (result is Resource<AppItemModel>.Error err) { }
        if (result is Resource<AppItemModel>.Success success)
        {
            Item = success.Data!;
        }
    }
}
