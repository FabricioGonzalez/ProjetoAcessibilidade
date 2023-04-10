using System;

namespace QuestPDF.Elements.Text.Calculation
{
    public class TextMeasurementResult
    {
        public float Width
        {
            get;
            set;
        }

        public float Height => Math.Abs(value: Descent) + Math.Abs(value: Ascent);

        public float Ascent
        {
            get;
            set;
        }

        public float Descent
        {
            get;
            set;
        }

        public float LineHeight
        {
            get;
            set;
        }

        public int StartIndex
        {
            get;
            set;
        }

        public int EndIndex
        {
            get;
            set;
        }

        public int NextIndex
        {
            get;
            set;
        }

        public int TotalIndex
        {
            get;
            set;
        }

        public bool IsLast => NextIndex == EndIndex;
    }
}