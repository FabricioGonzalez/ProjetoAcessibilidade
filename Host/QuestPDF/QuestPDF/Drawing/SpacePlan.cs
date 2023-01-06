using System.Collections.Generic;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing
{
    public readonly struct SpacePlan
    {
        public readonly SpacePlanType Type;
        public readonly float Width;
        public readonly float Height;

        public SpacePlan(SpacePlanType type, float width, float height)
        {
            Type = type;
            Width = width;
            Height = height;
        }

        public static SpacePlan Wrap() => new SpacePlan(SpacePlanType.Wrap, 0, 0);
        
        public static SpacePlan PartialRender(float width, float height) => new SpacePlan(SpacePlanType.PartialRender, width, height);

        public static SpacePlan PartialRender(Size size) => PartialRender(size.Width, size.Height);
        
        public static SpacePlan FullRender(float width, float height) => new SpacePlan(SpacePlanType.FullRender, width, height);

        public static SpacePlan FullRender(Size size) => FullRender(size.Width, size.Height);

        public override string ToString()
        {
            if (Type == SpacePlanType.Wrap)
                return Type.ToString();
            
            return $"{Type} (Width: {Width:N3}, Height: {Height:N3})";
        }

        public static implicit operator Size(SpacePlan spacePlan)
        {
            return new Size(spacePlan.Width, spacePlan.Height);
        }
    }
}