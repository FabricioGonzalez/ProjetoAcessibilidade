using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;
public class AppFormDataItemCheckboxModel : IAppFormDataItemContract
{
    public AppFormDataItemCheckboxModel(string id,
        string topic,
        AppFormDataType type = AppFormDataType.Checkbox)
        : base(id: id, topic: topic, type: type)
    {

    }
    public List<AppFormDataItemCheckboxChildModel> Children
    {
        get; set;
    } = new List<AppFormDataItemCheckboxChildModel>();
}
