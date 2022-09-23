using AppUsecases.Contracts.Entity;

using ReactiveUI;

namespace AppWinui.AppCode.Project.States.ProjectItems.Text;
public class AppFormDataItemTextModel : ReactiveObject, IAppFormDataItemContract
{
    public string Topic { get; set; } = "";
    public AppFormDataTypeEnum Type { get; set; } = AppFormDataTypeEnum.Text;
    private string _textData = "";
    public string TextData
    {
        get => _textData;
        set => this.RaiseAndSetIfChanged(
            ref _textData,
            value, 
            nameof(TextData));
    }

#nullable enable
    public string? MeasurementUnit { get; set; } = null;
#nullable disable
}
