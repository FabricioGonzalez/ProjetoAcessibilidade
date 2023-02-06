using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems;
public interface IAppFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public AppFormDataType Type
    {
        get; set;
    }
}
