using System.Collections.Generic;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class PreviewerPicture
    {
        public PreviewerPicture(
            SKPicture picture
            , Size size
        )
        {
            Picture = picture;
            Size = size;
        }

        public SKPicture Picture
        {
            get;
            set;
        }

        public Size Size
        {
            get;
            set;
        }
    }

    internal class SkiaPictureCanvas : SkiaCanvasBase
    {
        private SKPictureRecorder? PictureRecorder
        {
            get;
            set;
        }

        private Size? CurrentPageSize
        {
            get;
            set;
        }

        public ICollection<PreviewerPicture> Pictures
        {
            get;
        } = new List<PreviewerPicture>();

        public override void BeginDocument() => Pictures.Clear();

        public override void BeginPage(
            Size size
        )
        {
            CurrentPageSize = size;
            PictureRecorder = new SKPictureRecorder();

            Canvas = PictureRecorder.BeginRecording(cullRect: new SKRect(left: 0, top: 0, right: size.Width
                , bottom: size.Height));
        }

        public override void EndPage()
        {
            var picture = PictureRecorder?.EndRecording();

            if (picture != null && CurrentPageSize.HasValue)
            {
                Pictures.Add(item: new PreviewerPicture(picture: picture, size: CurrentPageSize.Value));
            }

            PictureRecorder?.Dispose();
            PictureRecorder = null;
        }

        public override void EndDocument()
        {
        }
    }

    public abstract class SkiaCanvasBase
        : ICanvas
            , IRenderingCanvas
    {
        public SKCanvas Canvas
        {
            get;
            set;
        }

        public void Translate(
            Position vector
        ) => Canvas.Translate(dx: vector.X, dy: vector.Y);

        public void DrawRectangle(
            Position vector
            , Size size
            , string color
        )
        {
            if (size.Width < Size.Epsilon || size.Height < Size.Epsilon)
            {
                return;
            }

            var paint = color.ColorToPaint();
            Canvas.DrawRect(x: vector.X, y: vector.Y, w: size.Width, h: size.Height, paint: paint);
        }

        public void DrawText(
            string text
            , Position vector
            , TextStyle style
        ) => Canvas.DrawText(text: text, x: vector.X, y: vector.Y, paint: style.ToPaint());

        public void DrawImage(
            SKImage image
            , Position vector
            , Size size
        ) => Canvas.DrawImage(image: image
            , dest: new SKRect(left: vector.X, top: vector.Y, right: size.Width, bottom: size.Height));

        public void DrawHyperlink(
            string url
            , Size size
        ) => Canvas.DrawUrlAnnotation(rect: new SKRect(left: 0, top: 0, right: size.Width, bottom: size.Height)
            , value: url);

        public void DrawSectionLink(
            string sectionName
            , Size size
        ) => Canvas.DrawLinkDestinationAnnotation(
            rect: new SKRect(left: 0, top: 0, right: size.Width, bottom: size.Height), value: sectionName);

        public void DrawSection(
            string sectionName
        ) => Canvas.DrawNamedDestinationAnnotation(point: new SKPoint(x: 0, y: 0), value: sectionName);

        public void Rotate(
            float angle
        ) => Canvas.RotateDegrees(degrees: angle);

        public void Scale(
            float scaleX
            , float scaleY
        ) => Canvas.Scale(sx: scaleX, sy: scaleY);

        public abstract void BeginDocument();
        public abstract void EndDocument();

        public abstract void BeginPage(
            Size size
        );

        public abstract void EndPage();
    }
}