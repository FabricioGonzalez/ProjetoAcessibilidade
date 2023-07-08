using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDFReport.Models;
using SkiaSharp;

namespace QuestPDFReport.Components;

internal class CheckboxItemComponent : IComponent
{
    public readonly CheckboxModel item;

    public CheckboxItemComponent(
        CheckboxModel checkbox
    )
    {
        item = checkbox;
    }

    public void Compose(
        IContainer container
    ) =>
        container
            .Height(20)
            .Row(row =>
            {
                row.Spacing(5);

                row
                    .ConstantItem(16)
                    .Layers(layers =>
                    {
                        layers
                            .PrimaryLayer()
                            .Canvas((
                                canvas
                                , size
                            ) =>
                            {
                                DrawRoundedRectangle(Colors.White, false);
                                DrawRoundedRectangle(Colors.Black, true);

                                if (item.IsChecked)
                                {
                                    DrawLine(Colors.Black, true, 6, 4, 10, 12);
                                    DrawLine(Colors.Black, true, 9.80f, 12.0f, 15.5f
                                        , -1f);
                                }


                                void DrawLine(
                                    string color
                                    , bool isStroke
                                    , float fromX
                                    , float fromY
                                    , float toX
                                    , float toY
                                )
                                {
                                    using var paint = new SKPaint
                                    {
                                        Color = SKColor.Parse(color), IsStroke = isStroke, StrokeWidth = 1
                                        , IsAntialias = true
                                    };

                                    canvas.DrawLine(new SKPoint(fromX, fromY), new SKPoint(toX, toY)
                                        , paint);
                                }

                                void DrawRoundedRectangle(
                                    string color
                                    , bool isStroke
                                )
                                {
                                    using var paint = new SKPaint
                                    {
                                        Color = SKColor.Parse(color), IsStroke = isStroke, StrokeWidth = 1
                                        , IsAntialias = true
                                    };

                                    canvas.DrawRoundRect(4, 2, 12, 12, 2, 4, paint);
                                }
                            });
                    });

                row.AutoItem()
                    .Text(item.Value);
            });
}