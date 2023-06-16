using Common;

using Core.Entities.Solution.Project.AppItem;

using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.SystemItems;

public sealed record SaveSystemProjectItemContentCommand(
    AppItemModel AppItem
    , string ItemPath
) : IRequest<Resource<Empty>>;

public class SaveSystemProjectItemContentCommandHandler
    : IHandler<SaveSystemProjectItemContentCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository _repository;
    public SaveSystemProjectItemContentCommandHandler(IProjectItemContentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Resource<Empty>> HandleAsync(
        SaveSystemProjectItemContentCommand command
        , CancellationToken cancellation
    )
    {
        await _repository.SaveSystemProjectItemContentSerealizer(
            dataToWrite: command.AppItem
            , filePathToWrite: command.ItemPath);

        return new Resource<Empty>.Success(Data: Empty.Value);
    }
}