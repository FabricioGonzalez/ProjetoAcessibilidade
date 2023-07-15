using System.Reactive;
using System.Threading;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Features.Project.ViewModels.EditingItemBody;
using ProjectAvalonia.Presentation.Interfaces;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;
using ReactiveUI;
using Splat;

namespace ProjectAvalonia.Features.Project.ViewModels.Components;

public class EditingItemViewModel
    : ViewModelBase
        , IEditingItemViewModel
{
    private readonly IMediator _mediator;

    public EditingItemViewModel(
        string itemName
        , string id
        , string itemPath
        , IEditingBodyViewModel body
        , bool isSaved = true
        , string templateName = null
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
                    var itemModel = (Body as EditingBodyViewModel).ToAppModel();
                    itemModel.ItemName = ItemName;
                    itemModel.TemplateName = TemplateName;

                    await _mediator
                        .Send(
                            new SaveProjectItemContentCommand(
                                itemModel, ItemPath),
                            CancellationToken.None);
                }
            });

        Body = body;
        TemplateName = templateName;
    }

    public string TemplateName
    {
        get;
    }

    public string ItemPath
    {
        get;
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

    public ReactiveCommand<Unit, Unit> CloseItemCommand
    {
        get;
    }

    public ReactiveCommand<Unit, Unit> SaveItemCommand
    {
        get;
    }
}