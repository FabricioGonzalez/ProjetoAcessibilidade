using System.Reactive;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Domain.Contracts;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels.Components;

public class SolutionEditItemViewModel
    : ReactiveObject
        , IEditingItemViewModel
{
    private readonly IMediator _mediator;

    public SolutionEditItemViewModel(
        string itemName
        , string id
        , string itemPath
        , ISolutionEditingBody body
        , bool isSaved = true
    )
    {
        _mediator ??= Locator.Current.GetService<IMediator>();

        ItemName = itemName;
        ItemPath = itemPath;
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
}