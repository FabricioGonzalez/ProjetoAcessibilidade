using System.Reactive;
using System.Threading;

using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Presentation.Interfaces;

using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels.Components;

public class EditingItemViewModel : ViewModelBase, IEditingItemViewModel
{

    private readonly IMediator _mediator;
    public EditingItemViewModel(string itemName, string id, string itemPath, IEditingBodyViewModel body, bool isSaved = true, string templateName = null)
    {
        _mediator ??= Locator.Current.GetService<IMediator>();

        ItemName = itemName;
        ItemPath = itemPath;
        Id = id;
        IsSaved = isSaved;
        CloseItemCommand = ReactiveCommand.Create(execute: () =>
        {
        });
        SaveItemCommand = ReactiveCommand.CreateFromTask(
            execute: async () =>
            {
                if (Body is not null)
                {
                    var itemModel = Body.ToAppModel();
                    itemModel.ItemName = ItemName;
                    itemModel.TemplateName = TemplateName;

                    await _mediator
                        .Send(
                             request: new SaveProjectItemContentCommand(
                                AppItem: itemModel, ItemPath: ItemPath),
                            cancellation: CancellationToken.None);
                }
            });

        Body = body;
        TemplateName = templateName;
    }

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


        
    }

    public ICommand SaveItemCommand
    {
        get;
    }

    public AppModelState EditingItem => _editingItemsStore?.Item;*/
    public string ItemName
    {
        get;
    }
    public string ItemPath
    {
        get;
    }
    public string TemplateName
    {
        get;
    }

    public string Id
    {
        get;
    }

    public bool IsSaved
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }



    public IEditingBodyViewModel Body
    {
        get;
    }
}