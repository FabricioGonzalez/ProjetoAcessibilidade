using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;

using ProjetoAcessibilidade.Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;

public class AppFormDataItemCheckboxModel : IAppFormDataItemContract
{
    public AppFormDataItemCheckboxModel(
        string id
        , string topic
        , AppFormDataType type = AppFormDataType.Checkbox
    )
        : base(id: id, topic: topic, type: type)
    {
    }

    public IEnumerable<AppFormDataItemCheckboxChildModel> Children
    {
        get;
        set;
    } = Enumerable.Empty<AppFormDataItemCheckboxChildModel>();
}