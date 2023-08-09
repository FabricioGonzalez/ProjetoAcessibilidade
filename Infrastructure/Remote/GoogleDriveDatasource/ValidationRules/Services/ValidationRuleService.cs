using AppRepositories.ValidationRules.Contracts;

namespace GoogleDriveDatasource.ValidationRules.Services;

public class ValidationRuleService
{
    private readonly IValidationRuleDatasource _datasource;

    public ValidationRuleService(
        IValidationRuleDatasource datasource
    )
    {
        _datasource = datasource;
    }
}