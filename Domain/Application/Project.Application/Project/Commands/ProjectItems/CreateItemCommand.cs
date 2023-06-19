using Common;

using ProjetoAcessibilidade.Domain.App.Models;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record CreateItemCommand(
    string ItemPath,
    string ItemName
) : IRequest<Resource<Empty>>;

public sealed class CreateItemCommandHandler : IHandler<CreateItemCommand, Resource<Empty>>
{
    private readonly IProjectItemContentRepository _repository;
    public CreateItemCommandHandler(IProjectItemContentRepository repository)
    {
        _repository = repository;
    }
    public async Task<Resource<Empty>> HandleAsync(
        CreateItemCommand command,
        CancellationToken cancellation
    )
    {
        var result = await _repository
            .GetSystemProjectItemContent(
                filePathToWrite: Path.Combine(
                    path1: Constants.AppItemsTemplateFolder,
                    path2: $"{command.ItemName}{Constants.AppProjectTemplateExtension}"));

        return result.Map((item) =>
        {
            item.ItemName = Path.GetFileNameWithoutExtension(command.ItemPath);
            return item;

        }).Map(async value =>
        {
            await _repository
            .SaveProjectItemContent(
                dataToWrite: value,
                filePathToWrite: command.ItemPath);
        })
        .Map<Resource<Empty>>((value) =>
        {
            return value.IsCompletedSuccessfully ? new Resource<Empty>.Success(Empty.Value) : new Resource<Empty>.Error(Message: "Houve um erro no processo");
        })
        .Reduce(() => new Resource<Empty>.Error(Message: "Houve um erro no processo"));




    }
}