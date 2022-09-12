using Core.Contracts;
using Core.Enums;

namespace SystemApplication.Services.UIOutputs;

public class FormDataItemTextModel : NotifierBaseClass, IFormDataItemContract
{
    public string Topic
    {
        get; set;
    }
    public FormDataItemTypeEnum Type { get; set; } = FormDataItemTypeEnum.Text;

    private string textData;
    public string TextData
    {
        get => textData;
        set => SetAtributeValue(ref textData, value);
    }


#nullable enable
    public string? MeasurementUnit
    {
        get; set;
    }

#nullable disable
}
