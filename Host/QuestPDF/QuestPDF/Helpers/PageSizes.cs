using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
    public class PageSize
    {
        public readonly float Height;
        public readonly float Width;

        public PageSize(
            float width
            , float height
            , Unit unit = Unit.Point
        )
        {
            Width = width.ToPoints(unit: unit);
            Height = height.ToPoints(unit: unit);
        }

        public static implicit operator Size(
            PageSize pageSize
        ) => new(width: pageSize.Width, height: pageSize.Height);
    }

    public struct PageSizes
    {
        public const int PointsPerInch = 72;

        public static PageSize A0 => new(width: 2384.2f, height: 3370.8f);
        public static PageSize A1 => new(width: 1684, height: 2384.2f);
        public static PageSize A2 => new(width: 1190.7f, height: 1684);
        public static PageSize A3 => new(width: 842, height: 1190.7f);
        public static PageSize A4 => new(width: 595.4f, height: 842);
        public static PageSize A5 => new(width: 419.6f, height: 595.4f);
        public static PageSize A6 => new(width: 297.7f, height: 419.6f);
        public static PageSize A7 => new(width: 209.8f, height: 297.7f);
        public static PageSize A8 => new(width: 147.4f, height: 209.8f);
        public static PageSize A9 => new(width: 104.9f, height: 147.4f);
        public static PageSize A10 => new(width: 73.7f, height: 104.9f);

        public static PageSize B0 => new(width: 2835, height: 4008.7f);
        public static PageSize B1 => new(width: 2004.3f, height: 2835);
        public static PageSize B2 => new(width: 1417.5f, height: 2004.3f);
        public static PageSize B3 => new(width: 1000.8f, height: 1417.5f);
        public static PageSize B4 => new(width: 708.8f, height: 1000.8f);
        public static PageSize B5 => new(width: 499, height: 708.8f);
        public static PageSize B6 => new(width: 354.4f, height: 499);
        public static PageSize B7 => new(width: 249.5f, height: 354.4f);
        public static PageSize B8 => new(width: 175.8f, height: 249.5f);
        public static PageSize B9 => new(width: 124.7f, height: 175.8f);
        public static PageSize B10 => new(width: 87.9f, height: 124.7f);

        public static PageSize Env10 => new(width: 683.2f, height: 294.8f);
        public static PageSize EnvC4 => new(width: 649.2f, height: 918.5f);
        public static PageSize EnvDL => new(width: 311.9f, height: 623.7f);

        public static PageSize Executive => new(width: 522, height: 756);
        public static PageSize Legal => new(width: 612, height: 1008);
        public static PageSize Letter => new(width: 612, height: 792);

        public static PageSize ARCH_A => new(width: 649.2f, height: 864.7f);
        public static PageSize ARCH_B => new(width: 864.7f, height: 1295.6f);
        public static PageSize ARCH_C => new(width: 1295.6f, height: 1729.3f);
        public static PageSize ARCH_D => new(width: 1729.3f, height: 2591.2f);
        public static PageSize ARCH_E => new(width: 2591.2f, height: 3455.9f);
        public static PageSize ARCH_E1 => new(width: 2160.3f, height: 3024.9f);
        public static PageSize ARCH_E2 => new(width: 1871.1f, height: 2735.8f);
        public static PageSize ARCH_E3 => new(width: 1944.8f, height: 2809.5f);
    }

    public static class PageSizeExtensions
    {
        public static PageSize Portrait(
            this PageSize size
        ) => new(width: Math.Min(val1: size.Width, val2: size.Height)
            , height: Math.Max(val1: size.Width, val2: size.Height));

        public static PageSize Landscape(
            this PageSize size
        ) => new(width: Math.Max(val1: size.Width, val2: size.Height)
            , height: Math.Min(val1: size.Width, val2: size.Height));
    }
}