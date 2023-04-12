namespace QuestPDF.Infrastructure
{
    public readonly struct Position
    {
        public readonly float X;
        public readonly float Y;

        public static Position Zero => new(x: 0, y: 0);

        public Position(
            float x
            , float y
        )
        {
            X = x;
            Y = y;
        }

        public Position Reverse() => new(x: -X, y: -Y);

        public override string ToString() => $"(Left: {X:N3}, Top: {Y:N3})";
    }
}