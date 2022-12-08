using AppUsecases.Project.Enums;

namespace AppUsecases.Contracts.Entity;
public interface IAppFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public AppFormDataTypeEnum Type
    {
        get; set;
    }
}
