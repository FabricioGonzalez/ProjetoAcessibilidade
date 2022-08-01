using Core.Contracts;
using Core.Enums;

namespace Core.Models;

internal class FormDataItemTextModel : IFormDataItemContract
{
    public string Topic { get; set; } = "";
    public FormDataItemTypeEnum Type { get; set; } = FormDataItemTypeEnum.Text;
    public string TextData { get; set; } = "";

#nullable enable
    public string? MeasurementUnit { get; set; } = null;
#nullable disable
}
