using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public class Page : IComponent
    {
        public TextStyle DefaultTextStyle
        {
            get;
            set;
        } = new();

        public Size MinSize
        {
            get;
            set;
        } = PageSizes.A4;

        public Size MaxSize
        {
            get;
            set;
        } = PageSizes.A4;

        public float MarginLeft
        {
            get;
            set;
        }

        public float MarginRight
        {
            get;
            set;
        }

        public float MarginTop
        {
            get;
            set;
        }

        public float MarginBottom
        {
            get;
            set;
        }

        public string BackgroundColor
        {
            get;
            set;
        } = Colors.Transparent;

        public Element Background
        {
            get;
            set;
        } = Empty.Instance;

        public Element Foreground
        {
            get;
            set;
        } = Empty.Instance;

        public Element Header
        {
            get;
            set;
        } = Empty.Instance;

        public Element Content
        {
            get;
            set;
        } = Empty.Instance;

        public Element Footer
        {
            get;
            set;
        } = Empty.Instance;

        public void Compose(
            IContainer container
        )
        {
            container
                .Background(color: BackgroundColor)
                .Layers(handler: layers =>
                {
                    layers
                        .Layer()
                        .DebugPointer(elementTraceText: "Page background layer")
                        .Element(child: Background);

                    layers
                        .PrimaryLayer()
                        .MinWidth(value: MinSize.Width)
                        .MinHeight(value: MinSize.Height)
                        .MaxWidth(value: MaxSize.Width)
                        .MaxHeight(value: MaxSize.Height)
                        .PaddingLeft(value: MarginLeft)
                        .PaddingRight(value: MarginRight)
                        .PaddingTop(value: MarginTop)
                        .PaddingBottom(value: MarginBottom)
                        .DefaultTextStyle(textStyle: DefaultTextStyle)
                        .Decoration(handler: decoration =>
                        {
                            decoration
                                .Before()
                                .DebugPointer(elementTraceText: "Page header")
                                .Element(child: Header);

                            decoration
                                .Content()
                                .Element(handler: x =>
                                    IsClose(x: MinSize.Width, y: MaxSize.Width) ? x.ExtendHorizontal() : x)
                                .Element(handler: x =>
                                    IsClose(x: MinSize.Height, y: MaxSize.Height) ? x.ExtendVertical() : x)
                                .DebugPointer(elementTraceText: "Page content")
                                .Element(child: Content);

                            decoration
                                .After()
                                .DebugPointer(elementTraceText: "Page footer")
                                .Element(child: Footer);
                        });

                    layers
                        .Layer()
                        .DebugPointer(elementTraceText: "Page foreground layer")
                        .Element(child: Foreground);
                });

            bool IsClose(
                float x
                , float y
            )
            {
                return Math.Abs(value: x - y) < Size.Epsilon;
            }
        }
    }
}