using System.IO;
using System.Linq;
using System.Reactive;
using ProjectAvalonia.Common.ViewModels;
using ProjectAvalonia.Presentation.Interfaces;
using ReactiveUI;

namespace ProjectAvalonia.Features.Project.ViewModels.Components;

public class EditingItemViewModel
    : ViewModelBase
        , IEditingItemViewModel
{
    /*private readonly IMediator _mediator;*/

    public EditingItemViewModel(
        string itemName
        , string id
        , string itemPath
        , IEditingBodyViewModel body
        , bool isSaved = true
        , string templateName = null
    )
    {
        /*_mediator ??= Locator.Current.GetService<IMediator>();*/

        ItemName = itemName;
        ItemPath = itemPath;
        DisplayName = string.Join(separator: Path.DirectorySeparatorChar,
            value: ItemPath.Split(Path.DirectorySeparatorChar)[^3 ..^1]
                .Append(Path.GetFileNameWithoutExtension(itemPath)).ToArray());

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
                    /*var itemModel = (Body as EditingBodyViewModel).ToAppModel();
                    itemModel.ItemName = ItemName;
                    itemModel.TemplateName = TemplateName;

                    await _mediator
                        .Send(
                            new SaveProjectItemContentCommand(
                                itemModel, ItemPath),
                            CancellationToken.None);*/
                }
            });

        Body = body;
        TemplateName = templateName;
    }

    public string DisplayName
    {
        get;
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