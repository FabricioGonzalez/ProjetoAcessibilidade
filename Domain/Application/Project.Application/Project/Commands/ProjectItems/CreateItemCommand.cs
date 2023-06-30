using Common;
using Common.Models;
using Common.Result;

using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record CreateItemCommand(
    string ItemPath,
    string ItemName
) : IRequest<Result<Empty>>;

public sealed class CreateItemCommandHandler : IHandler<CreateItemCommand, Result<Empty>>
{
    private readonly IProjectItemContentRepository _repository;
    public CreateItemCommandHandler(IProjectItemContentRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<Empty>> HandleAsync(
        CreateItemCommand command,
        CancellationToken cancellation
    )
    {
        return await (await _repository
           .GetSystemProjectItemContent(
               filePathToWrite: Path.Combine(
                   path1: Constants.AppItemsTemplateFolder,
                   path2: $"{command.ItemName}{Constants.AppProjectTemplateExtension}")))
                   .MatchAsync(async success =>
                   {
                       success.TemplateName = success.ItemName;
                       success.ItemName = Path.GetFileNameWithoutExtension(command.ItemPath);

                       await _repository.SaveProjectItemContent(dataToWrite: success, filePathToWrite: command.ItemPath);

                       return Result<Empty>.Success(Empty.Value);
                   },
                   async failure => await Task.Run(() => Result<Empty>.Failure(failure)));
    }
}