using ProjectAvalonia.Common.ViewModels;

namespace ProjectAvalonia.Features.Project.ViewModels.Components;

public class EditingItemViewModel : ViewModelBase
{
    /*private readonly ICommandDispatcher _commandDispatcher;
    private readonly EditingItemsStore _editingItemsStore;

    public EditingItemViewModel()
    {
        _editingItemsStore ??= Locator.Current.GetService<EditingItemsStore>();

        _commandDispatcher ??= Locator.Current.GetService<ICommandDispatcher>();

        this.WhenAnyValue(property1: vm => vm._editingItemsStore.CurrentSelectedItem)
            .WhereNotNull()
            .Subscribe(onNext: async prop =>
            {
                _editingItemsStore.Item = await _editingItemsStore.EditItem(item: prop);
            });


        SaveItemCommand = ReactiveCommand.CreateFromTask<AppModelState?>(
            execute: async appModel =>
            {
                if (appModel is not null)
                {
                    var itemModel = appModel.ToAppModel();

                    await _commandDispatcher
                        .Dispatch<SaveProjectItemContentCommand, Resource<Empty>>(
                            command: new SaveProjectItemContentCommand(
                                AppItem: itemModel, ItemPath: _editingItemsStore.CurrentSelectedItem.ItemPath),
                            cancellation: CancellationToken.None);
                }
            });
    }

    public ICommand SaveItemCommand
    {
        get;
    }

    public AppModelState EditingItem => _editingItemsStore?.Item;*/
}