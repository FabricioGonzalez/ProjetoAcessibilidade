using Core.Enuns;

namespace Core.Entities.Solution.Project.AppItem.DataItems.Text;

public class AppFormDataItemTextModel : IAppFormDataItemContract
{
    public AppFormDataItemTextModel(
        string id
        , string topic
        , AppFormDataType type = AppFormDataType.Texto
        , string textData = ""
        , string? measurementUnit = null
    )
        : base(id: id, topic: topic, type: type)
    {
        TextData = textData;
        MeasurementUnit = measurementUnit;
    }

    public string TextData
    {
        get;
        set;
    } = "";

    public string? MeasurementUnit
    {
        get;
        set;
    }
}