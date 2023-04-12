using QuestPDF.Helpers;

namespace QuestPDF.Infrastructure
{
    public class TextStyle
    {
        public bool HasGlobalStyleApplied
        {
            get;
            private set;
        }

        public string? Color
        {
            get;
            set;
        }

        public string? BackgroundColor
        {
            get;
            set;
        }

        public string? FontFamily
        {
            get;
            set;
        }

        public float? Size
        {
            get;
            set;
        }

        public float? LineHeight
        {
            get;
            set;
        }

        public FontWeight? FontWeight
        {
            get;
            set;
        }

        public bool? IsItalic
        {
            get;
            set;
        }

        public bool? HasStrikethrough
        {
            get;
            set;
        }

        public bool? HasUnderline
        {
            get;
            set;
        }

        public object PaintKey
        {
            get;
            private set;
        }

        public object FontMetricsKey
        {
            get;
            private set;
        }

        public static TextStyle LibraryDefault => new()
        {
            Color = Colors.Black, BackgroundColor = Colors.Transparent, FontFamily = Fonts.Calibri, Size = 12
            , LineHeight = 1.2f, FontWeight = Infrastructure.FontWeight.Normal, IsItalic = false
            , HasStrikethrough = false, HasUnderline = false
        };

        public static TextStyle Default => new();

        public void ApplyGlobalStyle(
            TextStyle globalStyle
        )
        {
            if (HasGlobalStyleApplied)
            {
                return;
            }

            HasGlobalStyleApplied = true;

            ApplyParentStyle(parentStyle: globalStyle);
            PaintKey ??= (FontFamily, Size, FontWeight, IsItalic, Color);
            FontMetricsKey ??= (FontFamily, Size, FontWeight, IsItalic);
        }

        public void ApplyParentStyle(
            TextStyle parentStyle
        )
        {
            Color ??= parentStyle.Color;
            BackgroundColor ??= parentStyle.BackgroundColor;
            FontFamily ??= parentStyle.FontFamily;
            Size ??= parentStyle.Size;
            LineHeight ??= parentStyle.LineHeight;
            FontWeight ??= parentStyle.FontWeight;
            IsItalic ??= parentStyle.IsItalic;
            HasStrikethrough ??= parentStyle.HasStrikethrough;
            HasUnderline ??= parentStyle.HasUnderline;
        }

        public void OverrideStyle(
            TextStyle parentStyle
        )
        {
            Color = parentStyle.Color ?? Color;
            BackgroundColor = parentStyle.BackgroundColor ?? BackgroundColor;
            FontFamily = parentStyle.FontFamily ?? FontFamily;
            Size = parentStyle.Size ?? Size;
            LineHeight = parentStyle.LineHeight ?? LineHeight;
            FontWeight = parentStyle.FontWeight ?? FontWeight;
            IsItalic = parentStyle.IsItalic ?? IsItalic;
            HasStrikethrough = parentStyle.HasStrikethrough ?? HasStrikethrough;
            HasUnderline = parentStyle.HasUnderline ?? HasUnderline;
        }

        public TextStyle Clone()
        {
            var clone = (TextStyle)MemberwiseClone();
            clone.HasGlobalStyleApplied = false;
            return clone;
        }
    }
}