using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
    public readonly struct SpacePlan
    {
        public readonly SpacePlanType Type;
        public readonly float Width;
        public readonly float Height;

        public SpacePlan(
            SpacePlanType type
            , float width
            , float height
        )
        {
            Type = type;
            Width = width;
            Height = height;
        }

        public static SpacePlan Wrap() => new(type: SpacePlanType.Wrap, width: 0, height: 0);

        public static SpacePlan PartialRender(
            float width
            , float height
        ) => new(type: SpacePlanType.PartialRender, width: width, height: height);

        public static SpacePlan PartialRender(
            Size size
        ) => PartialRender(width: size.Width, height: size.Height);

        public static SpacePlan FullRender(
            float width
            , float height
        ) => new(type: SpacePlanType.FullRender, width: width, height: height);

        public static SpacePlan FullRender(
            Size size
        ) => FullRender(width: size.Width, height: size.Height);

        public override string ToString()
        {
            if (Type == SpacePlanType.Wrap)
            {
                return Type.ToString();
            }

            return $"{Type} (Width: {Width:N3}, Height: {Height:N3})";
        }

        public static implicit operator Size(
            SpacePlan spacePlan
        ) => new(width: spacePlan.Width, height: spacePlan.Height);
    }
}