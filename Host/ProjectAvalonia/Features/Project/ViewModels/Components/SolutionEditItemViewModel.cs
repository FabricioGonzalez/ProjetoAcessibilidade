using System.Reactive;

using ProjectAvalonia.Features.Project.Services;
using ProjectAvalonia.Presentation.Interfaces;

using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Components;

public class SolutionEditItemViewModel
    : ReactiveObject
        , IEditingItemViewModel
{
    public SolutionEditItemViewModel(
        string itemName
        , string id
        , string itemPath
        , ISolutionEditingBody body
        ,EditingItemsNavigationService editableItemsNavigationService
        , bool isSaved = true
    )
    {

        ItemName = itemName;
        ItemPath = itemPath;
        DisplayName = itemName;
        Id = id;
        IsSaved = isSaved;

        _editableItemsNavigationService = editableItemsNavigationService;

        CloseItemCommand = ReactiveCommand.Create(() =>
        {
                _editableItemsNavigationService?.RemoveItem(this);
        });
        SaveItemCommand = ReactiveCommand.CreateFromTask(
            async () =>
            {
                if (Body is not null)
                {
                    /*var itemModel = Body.ToAppModel();
                    itemModel.ItemName = ItemName;
                    itemModel.TemplateName = TemplateName;

                    await _mediator
                        .Send(
                            request: new SaveProjectItemContentCommand(
                                AppItem: itemModel, ItemPath: ItemPath),
                            cancellation: CancellationToken.None);*/
                }
            });

        Body = body;
    }

    public IEditingBody Body
    {
        get;
        set;
    }

    public string ItemName
    {
        get; set;
    }

    private string _displayName = "";

    private readonly EditingItemsNavigationService _editableItemsNavigationService;

    public string DisplayName
    {
        get => _displayName; set => this.RaiseAndSetIfChanged(ref _displayName, value);
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

    public string ItemPath
    {
        get; set;
    }

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;init;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }

    public void Dispose()
    {

    }
}