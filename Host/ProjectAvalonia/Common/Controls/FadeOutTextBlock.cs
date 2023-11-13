using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace ProjectAvalonia.Common.Controls;

public class FadeOutTextBlock : TextBlock
{
    private static readonly IBrush FadeoutOpacityMask = new LinearGradientBrush
    {
        StartPoint = new RelativePoint(x: 0, y: 0, unit: RelativeUnit.Relative)
        , EndPoint = new RelativePoint(x: 1, y: 0, unit: RelativeUnit.Relative), GradientStops =
        {
            new GradientStop { Color = Colors.White, Offset = 0 }
            , new GradientStop { Color = Colors.White, Offset = 0.7 }
            , new GradientStop { Color = Colors.Transparent, Offset = 0.9 }
        }
    }.ToImmutable();

    private static readonly IBrush OpacityMask = new LinearGradientBrush
    {
        StartPoint = new RelativePoint(x: 0, y: 0, unit: RelativeUnit.Relative)
        , EndPoint = new RelativePoint(x: 1, y: 0, unit: RelativeUnit.Relative), GradientStops =
        {
            new GradientStop { Color = Colors.White, Offset = 0 }, new GradientStop { Color = Colors.White, Offset = 1 }
        }
    }.ToImmutable();

    internal TextBlock? TrimmedTextBlock
    {
        get;
        set;
    }

    protected override void RenderTextLayout(
        DrawingContext context
        , Point origin
    )
    {
        if (TrimmedTextBlock is null)
        {
            base.RenderTextLayout(context: context, origin: origin);
        }
        else
        {
            var hasCollapsed = TrimmedTextBlock.TextLayout.TextLines[0].HasCollapsed;
            if (hasCollapsed)
            {
                using var _ = context.PushOpacityMask(mask: FadeoutOpacityMask, bounds: Bounds);
                TextLayout.Draw(context: context, origin: origin + new Point(x: TextLayout.OverhangLeading, y: 0));
            }
            else
            {
                using var _ = context.PushOpacityMask(mask: OpacityMask, bounds: Bounds);
                TextLayout.Draw(context: context, origin: origin + new Point(x: TextLayout.OverhangLeading, y: 0));
            }
        }
    }
}