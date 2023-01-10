using App.Core.Enuns;

namespace App.Core.Entities.Solution.Project.AppItem.DataItems.Text;
public class AppFormDataItemTextModel : IAppFormDataItemContract
{
    public string Topic { get; set; } = "";
    public AppFormDataType Type { get; set; } = AppFormDataType.Texto;
    public string TextData { get; set; } = "";

#nullable enable
    public string? MeasurementUnit { get; set; } = null;
#nullable disable
}
