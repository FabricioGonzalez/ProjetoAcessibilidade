using AppUsecases.Contracts.Entity;

namespace AppUsecases.Entities.AppFormDataItems.Text;
public class AppFormDataItemTextModel : IAppFormDataItemContract
{
    public string Topic { get; set; } = "";
    public AppFormDataTypeEnum Type { get; set; } = AppFormDataTypeEnum.Text;
    public string TextData { get; set; } = "";

#nullable enable
    public string? MeasurementUnit { get; set; } = null;
#nullable disable
}
