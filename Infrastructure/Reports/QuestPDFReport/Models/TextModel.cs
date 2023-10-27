namespace QuestPDFReport.Models;

public class TextModel
{
    public TextModel(string text, string label, string measurementUnit)
    {
        Text = text;
        Label = label;
        MeasurementUnit = measurementUnit;
    }

    public string Text
    {
        get;
        set;
    }

    public string Label
    {
        get;
        set;
    }

    public string MeasurementUnit
    {
        get;
        set;
    }
}