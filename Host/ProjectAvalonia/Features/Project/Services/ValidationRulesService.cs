﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Common;

using ProjectAvalonia.Common.Logging;
using ProjectAvalonia.Features.Project.Mappings;
using ProjectAvalonia.Presentation.Interfaces;

using XmlDatasource.ValidationRules;
using XmlDatasource.ValidationRules.DTO;

namespace ProjectAvalonia.Features.Project.Services;

public sealed class ValidationRulesService
{
    private readonly ValidationRulesDatasourceImpl _validationRulesDatasource;

    public ValidationRulesService(
        ValidationRulesDatasourceImpl validationRulesDatasource
    )
    {
        _validationRulesDatasource = validationRulesDatasource;
    }

    public async Task CraeteRules(
        IEnumerable<IValidationRuleContainerState> rules
        , string itemName
    ) =>
        (await _validationRulesDatasource.CreateRules(rule: new ValidationItemRoot
        {
            Rules = rules.Select(it => it.ToValidationItem()).ToList()
        }, path: Path.Combine(path1: Constants.AppValidationRulesTemplateFolder, path2: $"{itemName}{Constants.AppProjectValidationTemplateExtension}")
        )).IfFail(fail => Logger.LogError(fail));

    public async Task<IEnumerable<IValidationRuleContainerState>> LoadRulesByPath(
        string? path
    ) =>
        (await _validationRulesDatasource.LoadRules(path))
        .Match(Succ: succ => succ.Rules.Select(it => it.ToValidationRuleContainerState())
            , Fail: fail =>
            {
                Logger.LogError(fail);
                return Enumerable.Empty<IValidationRuleContainerState>();
            });

    public async Task<IEnumerable<IValidationRuleContainerState>> LoadRulesByName(
        string name
    ) =>
        (await _validationRulesDatasource.LoadRules(Path.Combine(path1: Constants.AppValidationRulesTemplateFolder
            , path2: $"{name}{Constants.AppProjectValidationTemplateExtension}")))
        .Match(Succ: succ => succ.Rules.Select(it => it.ToValidationRuleContainerState())
            , Fail: fail =>
            {
                Logger.LogError(fail);
                return Enumerable.Empty<IValidationRuleContainerState>();
            });
}