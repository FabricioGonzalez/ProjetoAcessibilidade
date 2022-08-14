using Core.Contracts;
using Core.Enums;

namespace Core.Models;
public class FormDataItemObservation : IFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public FormDataItemTypeEnum Type { get; set; } = FormDataItemTypeEnum.Observation;
    public string value
    {
        get; set;
    }
}
