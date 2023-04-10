namespace QuestPDF.Infrastructure
{
    public readonly struct Size
    {
        public const float Epsilon = 0.001f;
        public const float Infinity = 14_400;

        public readonly float Width;
        public readonly float Height;

        public static Size Zero
        {
            get;
        } = new(width: 0, height: 0);

        public static Size Max
        {
            get;
        } = new(width: Infinity, height: Infinity);

        public Size(
            float width
            , float height
        )
        {
            Width = width;
            Height = height;
        }

        public override string ToString() => $"(Width: {Width:N3}, Height: {Height:N3})";
    }
}