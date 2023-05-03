using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Common;
using ProjectAvalonia.Features.NavBar;
using ProjectAvalonia.Features.TemplateEdit.ViewModels.Components;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Domain.App.Queries.Templates;
using ProjetoAcessibilidade.Domain.Contracts;
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
public partial class TemplateEditViewModel : NavBarItemViewModel, ITemplateEditViewModel
{
    private readonly IMediator _queryDispatcher;


    public TemplateEditViewModel()
    {
        _queryDispatcher = Locator.Current.GetService<IMediator>();

        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;

        AddNewItemCommand = ReactiveCommand.Create(execute: () =>
        {
        });
        LoadAllItems = ReactiveCommand.CreateFromTask(execute: loadItems, outputScheduler: RxApp.MainThreadScheduler);

        ExcludeItemCommand = ReactiveCommand.Create(execute: () =>
        {
        });
        RenameItemCommand = ReactiveCommand.Create(execute: () =>
        {
        });
        CommitItemCommand = ReactiveCommand.Create(execute: () =>
        {
        });
    }

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
    public ObservableCollection<IEditableItemViewModel> Items
    {
        get;
        set;
    }

    public IEditableItemViewModel SelectedItem
    {
        get;
        set;
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

    private async Task loadItems() =>
        (await _queryDispatcher?
            .Dispatch(
                query: new GetAllTemplatesQuery(),
                cancellation: CancellationToken.None))
        ?.OnSuccess(onSuccessAction: success =>
        {
            Items = new ObservableCollection<IEditableItemViewModel>(collection: success
                ?.Data
                ?.Select(selector: item => new EditableItemViewModel
                {
                    Id = item.Id ?? Guid.NewGuid().ToString(),
                    ItemPath = item.ItemPath,
                    Name = "",
                    TemplateName = item.Name
                }) ?? Enumerable.Empty<EditableItemViewModel>());

            this.RaisePropertyChanged(propertyName: nameof(Items));
        })
        ?.OnError(onErrorAction: error =>
        {
        });
}