using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public static class TextStyleExtensions
    {
        private static TextStyle Mutate(
            this TextStyle style
            , Action<TextStyle> handler
        )
        {
            style = style.Clone();

            handler(obj: style);
            return style;
        }

        [Obsolete(message: "This element has been renamed since version 2022.3. Please use the FontColor method.")]
        public static TextStyle Color(
            this TextStyle style
            , string value
        ) => style.FontColor(value: value);

        public static TextStyle FontColor(
            this TextStyle style
            , string value
        ) => style.Mutate(handler: x => x.Color = value);

        public static TextStyle BackgroundColor(
            this TextStyle style
            , string value
        ) => style.Mutate(handler: x => x.BackgroundColor = value);

        [Obsolete(message: "This element has been renamed since version 2022.3. Please use the FontFamily method.")]
        public static TextStyle FontType(
            this TextStyle style
            , string value
        ) => style.FontFamily(value: value);

        public static TextStyle FontFamily(
            this TextStyle style
            , string value
        ) => style.Mutate(handler: x => x.FontFamily = value);

        [Obsolete(message: "This element has been renamed since version 2022.3. Please use the FontSize method.")]
        public static TextStyle Size(
            this TextStyle style
            , float value
        ) => style.FontSize(value: value);

        public static TextStyle FontSize(
            this TextStyle style
            , float value
        ) => style.Mutate(handler: x => x.Size = value);

        public static TextStyle LineHeight(
            this TextStyle style
            , float value
        ) => style.Mutate(handler: x => x.LineHeight = value);

        public static TextStyle Italic(
            this TextStyle style
            , bool value = true
        ) => style.Mutate(handler: x => x.IsItalic = value);

        public static TextStyle Strikethrough(
            this TextStyle style
            , bool value = true
        ) => style.Mutate(handler: x => x.HasStrikethrough = value);

        public static TextStyle Underline(
            this TextStyle style
            , bool value = true
        ) => style.Mutate(handler: x => x.HasUnderline = value);

        #region Weight

        public static TextStyle Weight(
            this TextStyle style
            , FontWeight weight
        ) => style.Mutate(handler: x => x.FontWeight = weight);

        public static TextStyle Thin(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.Thin);

        public static TextStyle ExtraLight(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.ExtraLight);

        public static TextStyle Light(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.Light);

        public static TextStyle NormalWeight(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.Normal);

        public static TextStyle Medium(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.Medium);

        public static TextStyle SemiBold(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.SemiBold);

        public static TextStyle Bold(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.Bold);

        public static TextStyle ExtraBold(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.ExtraBold);

        public static TextStyle Black(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.Black);

        public static TextStyle ExtraBlack(
            this TextStyle style
        ) => style.Weight(weight: FontWeight.ExtraBlack);

        #endregion
    }
}