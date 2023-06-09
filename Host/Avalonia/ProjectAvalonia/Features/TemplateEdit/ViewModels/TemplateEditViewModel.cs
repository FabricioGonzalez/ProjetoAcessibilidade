using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

using Common;
using Common.Optional;

using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;
using ProjectAvalonia.Presentation.Interfaces;
using ProjectAvalonia.Presentation.States;

using ProjetoAcessibilidade.Domain.App.Queries.Templates;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Queries.SystemItems;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Modelos",
    Caption = "Editar os modelos de relatórios do projeto",
    Order = 0,
    LocalizedTitle = "TemplateEditViewNavLabel",
    Category = "Templates",
    Searchable = true,
    Keywords = new[]
    {
        "Templates", "Edição", "Modelos"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_regular")]
public partial class TemplateEditViewModel
    : NavBarItemViewModel,
        ITemplateEditViewModel
{
    private readonly IMediator _mediator;


    public TemplateEditViewModel(
        ITemplateEditTabViewModel templateEditTab
    )
    {
        TemplateEditTab = templateEditTab;
        _mediator = Locator.Current.GetService<IMediator>()!;

        SetupCancel(
            enableCancel: false,
            enableCancelOnEscape: true,
            enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        this.WhenAnyValue(x => x.SelectedItem)
            .WhereNotNull()
            .InvokeCommand(LoadSelectedItem);


        AddNewItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        LoadAllItems = ReactiveCommand.CreateFromTask(
            execute: LoadItems,
            outputScheduler: RxApp.MainThreadScheduler);

        ExcludeItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        RenameItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
        CommitItemCommand = ReactiveCommand.Create(
            () =>
            {
            });
    }

    private ReactiveCommand<IEditableItemViewModel, Unit> LoadSelectedItem => ReactiveCommand.CreateFromTask<IEditableItemViewModel>(async (item) =>
    {
        var result = await _mediator.Send(
               new GetSystemProjectItemContentQuery(item.ItemPath),
               CancellationToken.None);

        result.OnSuccess(onSuccessAction: success =>
        {
            success
            ?.Data
            ?.ToAppStateFillable()
            .ToOption()
            .Map(val => TemplateEditTab.EditingItem = val);
        });
    });

    public override string? LocalizedTitle
    {
        get;
        protected set;
    } = null;

    /*private readonly TemplateItemsStore _itemsStore;
    private readonly ICommandDispatcher commandDispatcher;


    [AutoNotify] private int _selectedTab;

    public TemplateEditViewModel()
    {
        commandDispatcher = Locator.Current.GetService<ICommandDispatcher>();

        _itemsStore ??= Locator.Current.GetService<TemplateItemsStore>();

        SelectionMode = NavBarItemSelectionMode.Button;

        _selectedTab = 0;

        TemplateEditTab = new TemplateEditTabViewModel();

        AddNewItemCommand = ReactiveCommand.Create(() =>
        {
            _itemsStore?.AddItem(new ItemState { InEditMode = true });
        });

        ExcludeItemCommand = ReactiveCommand.CreateFromTask<ItemState>(async item =>
        {
            var dialog = new DeleteDialogViewModel(
                "O item seguinte será excluido ao confirmar. Deseja continuar?", "Deletar Item"
                , "");

            if ((await NavigateDialogAsync(dialog, NavigationTarget.CompactDialogScreen)).Result)
            {
                /*Logger.LogInfo(groupModels.Name);#1#
            }
        });

        RenameItemCommand = ReactiveCommand.Create<ItemState>(item =>
        {
            item.InEditMode = true;
        });

        CommitItemCommand = ReactiveCommand.Create<ItemState>(item =>
        {
            var result = Items.Single(file => file.Name == item.Name);

            result.ItemPath = Path
                .Combine(Constants.AppItemsTemplateFolder
                    , $"{result.Name}{Constants.AppProjectTemplateExtension}");

            var appItem = new AppItemModel
            {
                ItemName = result.Name, TemplateName = result.Name, Id = Guid.NewGuid().ToString(),
                LawList = new List<AppLawModel>(), FormData = new List<IAppFormDataItemContract>
                {
                    new AppFormDataItemObservationModel(
                        type: AppFormDataType.Observação,
                        topic: "Observações",
                        observation: "",
                        id: ""),
                    new AppFormDataItemImageModel(
                        topic: "Imagens",
                        id: "",
                        type: AppFormDataType.Image)
                    {
                        ImagesItems = new List<ImagesItem>()
                    }
                }
            };

            if (!IoHelpers.CheckIfFileExists(result.ItemPath))
            {
                commandDispatcher.Dispatch<SaveSystemProjectItemContentCommand, Resource<Empty>>(
                    new SaveSystemProjectItemContentCommand(appItem, result.ItemPath),
                    CancellationToken.None);
            }

            Logger.LogDebug(result.Name);
        });

        (CommitItemCommand as ReactiveCommand<ItemState, Unit>)?.ThrownExceptions
            .Subscribe(exception =>
            {
                _itemsStore?.RemoveItem(Items.Last());
            });

        Dispatcher.UIThread.Post(async () =>
        {
            await _itemsStore?.LoadSystemItems(GetCancellationToken());
        });
    }

    public ReadOnlyObservableCollection<ItemState> Items => _itemsStore?.ItemsCollection;

    public TemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public ICommand AddNewItemCommand
    {
        get;
    }

    public ICommand ExcludeItemCommand
    {
        get;
    }

    public ICommand RenameItemCommand
    {
        get;
    }

    public ICommand CommitItemCommand
    {
        get;
    }*/
    public ObservableCollection<IEditableItemViewModel>? Items
    {
        get;
        set;
    }
    private IEditableItemViewModel _selectedItem;
    public IEditableItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public ITemplateEditTabViewModel TemplateEditTab
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> AddNewItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> LoadAllItems
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> ExcludeItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> RenameItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CommitItemCommand
    {
        get;
    }

    private async Task LoadItems() =>
        (await _mediator
            .Send(
                request: new GetAllTemplatesQuery(),
                cancellation: CancellationToken.None)).OnSuccess(
            success =>
            {
                Items = new ObservableCollection<IEditableItemViewModel>(
                    success.Data
                        ?.Select(
                            item => new EditableItemViewModel
                            {
                                Id = item.Id ??
                                     Guid.NewGuid()
                                         .ToString(),
                                ItemPath = item.ItemPath,
                                Name = "",
                                TemplateName = item.Name
                            }) ??
                    Enumerable.Empty<EditableItemViewModel>());

                this.RaisePropertyChanged(nameof(Items));
            })
        .OnError(
            _ =>
            {
            });
}