using System;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class PageDescriptor
    {
        public Page Page
        {
            get;
        } = new();

        public void Size(
            float width
            , float height
            , Unit unit = Unit.Inch
        )
        {
            var pageSize = new PageSize(width: width, height: height, unit: unit);

            MinSize(pageSize: pageSize);
            MaxSize(pageSize: pageSize);
        }

        public void Size(
            PageSize pageSize
        )
        {
            MinSize(pageSize: pageSize);
            MaxSize(pageSize: pageSize);
        }

        public void ContinuousSize(
            float width
            , Unit unit = Unit.Point
        )
        {
            MinSize(pageSize: new PageSize(width: width.ToPoints(unit: unit), height: 0));
            MaxSize(pageSize: new PageSize(width: width.ToPoints(unit: unit), height: Infrastructure.Size.Max.Height));
        }

        public void MinSize(
            PageSize pageSize
        ) => Page.MinSize = pageSize;

        public void MaxSize(
            PageSize pageSize
        ) => Page.MaxSize = pageSize;

        public void MarginLeft(
            float value
            , Unit unit = Unit.Point
        ) => Page.MarginLeft = value.ToPoints(unit: unit);

        public void MarginRight(
            float value
            , Unit unit = Unit.Point
        ) => Page.MarginRight = value.ToPoints(unit: unit);

        public void MarginTop(
            float value
            , Unit unit = Unit.Point
        ) => Page.MarginTop = value.ToPoints(unit: unit);

        public void MarginBottom(
            float value
            , Unit unit = Unit.Point
        ) => Page.MarginBottom = value.ToPoints(unit: unit);

        public void MarginVertical(
            float value
            , Unit unit = Unit.Point
        )
        {
            MarginTop(value: value, unit: unit);
            MarginBottom(value: value, unit: unit);
        }

        public void MarginHorizontal(
            float value
            , Unit unit = Unit.Point
        )
        {
            MarginLeft(value: value, unit: unit);
            MarginRight(value: value, unit: unit);
        }

        public void Margin(
            float value
            , Unit unit = Unit.Point
        )
        {
            MarginVertical(value: value, unit: unit);
            MarginHorizontal(value: value, unit: unit);
        }

        public void DefaultTextStyle(
            TextStyle textStyle
        ) => Page.DefaultTextStyle = textStyle;

        public void DefaultTextStyle(
            Func<TextStyle, TextStyle> handler
        ) => DefaultTextStyle(textStyle: handler(arg: TextStyle.Default));

        public void PageColor(
            string color
        ) => Page.BackgroundColor = color;

        [Obsolete(message: "This element has been renamed since version 2022.3. Please use the PageColor method.")]
        public void Background(
            string color
        ) => PageColor(color: color);

        public IContainer Background()
        {
            var container = new Container();
            Page.Background = container;
            return container;
        }

        public IContainer Foreground()
        {
            var container = new Container();
            Page.Foreground = container;
            return container;
        }

        public IContainer Header()
        {
            var container = new Container();
            Page.Header = container;
            return container;
        }

        public IContainer Content()
        {
            var container = new Container();
            Page.Content = container;
            return container;
        }

        public IContainer Footer()
        {
            var container = new Container();
            Page.Footer = container;
            return container;
        }
    }

    public static class PageExtensions
    {
        public static IDocumentContainer Page(
            this IDocumentContainer document
            , Action<PageDescriptor> handler
        )
        {
            var descriptor = new PageDescriptor();
            handler(obj: descriptor);

            (document as DocumentContainer).Pages.Add(item: descriptor.Page);

            return document;
        }
    }
}