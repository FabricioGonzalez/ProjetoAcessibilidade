using System.Reactive;

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
        , bool isSaved = true
    )
    {

        ItemName = itemName;
        ItemPath = itemPath;
        DisplayName = itemName;
        Id = id;
        IsSaved = isSaved;
        CloseItemCommand = ReactiveCommand.Create(() =>
        {
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
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }

    public void Dispose()
    {

    }
}