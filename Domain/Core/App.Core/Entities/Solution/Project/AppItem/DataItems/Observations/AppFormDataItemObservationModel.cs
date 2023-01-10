using App.Core.Enuns;

namespace App.Core.Entities.Solution.Project.AppItem.DataItems.Observations;
public class AppFormDataItemObservationModel : IAppFormDataItemContract
{
    public string Topic
    {
        get;
        set;
    }
    public AppFormDataType Type
    {
        get;
        set;
    } = AppFormDataType.Observação;
    public string Observation
    {
        get; set;
    }
}
