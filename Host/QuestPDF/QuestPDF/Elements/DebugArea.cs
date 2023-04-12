using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Elements
{
    public class DebugArea : IComponent
    {
        public IElement? Child
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        } = Colors.Red.Medium;

        public void Compose(
            IContainer container
        )
        {
            var backgroundColor = SKColor.Parse(hexString: Color).WithAlpha(alpha: 50).ToString();

            container
                .Border(value: 1)
                .BorderColor(color: Color)
                .Layers(handler: layers =>
                {
                    layers.PrimaryLayer().Element(child: Child);
                    layers.Layer().Background(color: backgroundColor);

                    layers
                        .Layer()
                        .ShowIf(condition: !string.IsNullOrWhiteSpace(value: Text))
                        .AlignCenter()
                        .MinimalBox()
                        .Background(color: Colors.White)
                        .Padding(value: 2)
                        .Text(text: Text)
                        .FontColor(value: Color)
                        .FontFamily(value: Fonts.Consolas)
                        .FontSize(value: 8);
                });
        }
    }
}