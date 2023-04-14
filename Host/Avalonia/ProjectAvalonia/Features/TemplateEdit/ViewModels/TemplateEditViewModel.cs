using ProjectAvalonia.Features.NavBar;

namespace ProjectAvalonia.Features.TemplateEdit.ViewModels;

[NavigationMetaData(
    Title = "Modelos",
    Caption = "Editar os modelos de relatórios do projeto",
    Order = 0,
    Category = "Templates",
    Searchable = true,
    Keywords = new[]
    {
        "Templates", "Edição", "Modelos"
    },
    NavBarPosition = NavBarPosition.Top,
    NavigationTarget = NavigationTarget.HomeScreen,
    IconName = "edit_regular")]
public partial class TemplateEditViewModel : NavBarItemViewModel
{
    public TemplateEditViewModel()
    {
        SetupCancel(enableCancel: false, enableCancelOnEscape: true, enableCancelOnPressed: true);

        SelectionMode = NavBarItemSelectionMode.Button;
    }

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
}