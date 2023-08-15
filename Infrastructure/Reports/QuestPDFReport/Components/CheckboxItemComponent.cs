﻿using QuestPDF.Fluent;
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
                            .Background(
                                item.IsChecked
                                    ? Colors.Red
                                        .Lighten1
                                    : Colors
                                        .White)
                            .Canvas((
                                canvas
                                , size
                            ) =>
                            {
                                DrawRoundedRectangle(color: Colors.White, isStroke: false);
                                DrawRoundedRectangle(color: Colors.Black, isStroke: true);

                                if (item.IsChecked)
                                {
                                    DrawLine(color: Colors.Black, isStroke: true, fromX: 6, fromY: 4, toX: 10, toY: 12);
                                    DrawLine(color: Colors.Black, isStroke: true, fromX: 9.80f, fromY: 12.0f, toX: 15.5f
                                        , toY: -1f);
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

                                    canvas.DrawLine(p0: new SKPoint(x: fromX, y: fromY), p1: new SKPoint(x: toX, y: toY)
                                        , paint: paint);
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

                                    canvas.DrawRoundRect(x: 4, y: 2, w: 12, h: 12, rx: 2, ry: 4, paint: paint);
                                }
                            });
                    });

                row.AutoItem()
                    .Text(item.Value);
            });
}