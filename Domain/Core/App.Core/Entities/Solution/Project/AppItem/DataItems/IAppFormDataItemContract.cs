using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems;

public abstract class IAppFormDataItemContract
{
    public IAppFormDataItemContract(
        string id
        , string topic
        , AppFormDataType type
    )
    {
        Id = id;
        Topic = topic;
        Type = type;
    }

    public string Id
    {
        get;
        set;
    }

    public string Topic
    {
        get;
        set;
    }

    public AppFormDataType Type
    {
        get;
        set;
    }
}