using Common;
using Common.Models;
using LanguageExt.Common;
using ProjetoAcessibilidade.Domain.Contracts;
using ProjetoAcessibilidade.Domain.Project.Contracts;

namespace ProjetoAcessibilidade.Domain.Project.Commands.ProjectItems;

public sealed record CreateItemCommand(
    string ItemPath
    , string ItemName
) : IRequest<Result<Empty>>;

public sealed class CreateItemCommandHandler : IHandler<CreateItemCommand, Result<Empty>>
{
    private readonly IProjectItemContentRepository _repository;

    public CreateItemCommandHandler(
        IProjectItemContentRepository repository
    )
    {
        _repository = repository;
    }

    public async Task<Result<Empty>> HandleAsync(
        CreateItemCommand command
        , CancellationToken cancellation
    ) =>
        await (await _repository
                .GetSystemProjectItemContent(
                    Path.Combine(
                        Constants.AppItemsTemplateFolder,
                        $"{command.ItemName}{Constants.AppProjectTemplateExtension}")))
            .Match(async success =>
                {
                    success.TemplateName = success.ItemName;
                    success.ItemName = Path.GetFileNameWithoutExtension(command.ItemPath);

                    await _repository.SaveProjectItemContent(success, command.ItemPath);

                    return new Result<Empty>(Empty.Value);
                },
                async failure => await Task.Run(() => new Result<Empty>(failure)));
}