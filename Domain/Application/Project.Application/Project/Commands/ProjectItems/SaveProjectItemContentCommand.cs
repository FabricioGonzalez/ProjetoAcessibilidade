using Common;

using Core.Entities.Solution.Project.AppItem;

using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record SaveProjectItemContentCommand(
    AppItemModel AppItem
    , string ItemPath
) : IRequest<Resource<Empty>>;

public sealed class SaveProjectItemContentCommandHandler
    : IHandler<SaveProjectItemContentCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository _repository;
    public SaveProjectItemContentCommandHandler(IProjectItemContentRepository repository)
    {
        _repository = repository;
    }
    public async Task<Resource<Empty>> HandleAsync(
        SaveProjectItemContentCommand command
        , CancellationToken cancellation
    )
    {
        await _repository.SaveProjectItemContent(dataToWrite: command.AppItem, filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}