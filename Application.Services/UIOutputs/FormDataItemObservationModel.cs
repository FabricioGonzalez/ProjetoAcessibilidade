using Core.Contracts;
using Core.Enums;

namespace SystemApplication.Services.UIOutputs;
public class FormDataItemObservationModel : NotifierBaseClass, IFormDataItemContract
{
    public FormDataItemTypeEnum Type
    {
        get;
        set;
    }
    public string Topic
    {
        get;
        set;
    }

    private string observation;
    public string Observation
    {
        get => observation;
        set => SetAtributeValue(ref observation, value);
    }
}
