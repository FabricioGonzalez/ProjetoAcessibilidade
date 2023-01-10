using App.Core.Enuns;

namespace App.Core.Entities.Solution.Project.AppItem.DataItems;
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
