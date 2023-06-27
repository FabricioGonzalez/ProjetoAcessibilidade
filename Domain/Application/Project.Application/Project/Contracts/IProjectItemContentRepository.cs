﻿using Common.Optional;
using Common.Result;

using Core.Entities.Solution.Project.AppItem;

namespace ProjetoAcessibilidade.Domain.Project.Contracts;

public interface IProjectItemContentRepository
{
    public Task<AppItemModel?> GetProjectItemContent(
        string filePathToWrite
    );

    public Task<Optional<AppItemModel>> GetSystemProjectItemContent(
        string filePathToWrite
    );

    public Task<Result<AppItemModel>> GetSystemProjectItemContentSerealizer(
        string filePathToWrite
    );

    public Task SaveProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    );

    public Task SaveSystemProjectItemContent(
        AppItemModel dataToWrite
        , string filePathToWrite
    );

    public Task SaveSystemProjectItemContentSerealizer(
        AppItemModel dataToWrite
        , string filePathToWrite
    );
}