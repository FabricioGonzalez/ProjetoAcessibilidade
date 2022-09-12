using Core.Contracts;
using Core.Enums;

namespace Core.Models;
public class FormDataItemObservationModel : IFormDataItemContract
{
    public string Topic
    {
        get;
        set;
    }
    public FormDataItemTypeEnum Type
    {
        get;
        set;
    }
    public string Observation;
}
