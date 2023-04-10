using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Placeholder : IComponent
    {
        private static readonly byte[] ImageData;

        static Placeholder()
        {
            ImageData = Helpers.Helpers.LoadEmbeddedResource(resourceName: "QuestPDF.Resources.ImagePlaceholder.png");
        }

        public string Text
        {
            get;
            set;
        }

        public void Compose(
            IContainer container
        ) =>
            container
                .Background(color: Colors.Grey.Lighten2)
                .Padding(value: 5)
                .AlignMiddle()
                .AlignCenter()
                .Element(handler: x =>
                {
                    if (string.IsNullOrWhiteSpace(value: Text))
                    {
                        x.MaxHeight(value: 32).Image(imageData: ImageData, scaling: ImageScaling.FitArea);
                    }
                    else
                    {
                        x.Text(text: Text).FontSize(value: 14);
                    }
                });
    }
}