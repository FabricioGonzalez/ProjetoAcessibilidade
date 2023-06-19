using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Text;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;

public class AppFormDataItemCheckboxChildModel
{
    public AppFormDataItemCheckboxChildModel(
        string id
        , string topic
    )
    {
        Id = id;
        Topic = topic;
        Options = new List<AppOptionModel>();
        TextItems = new List<AppFormDataItemTextModel>();
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

    public IEnumerable<AppOptionModel> Options
    {
        get;
        set;
    }

    public IEnumerable<AppFormDataItemTextModel> TextItems
    {
        get;
        set;
    }
}