namespace QuestPDF.Elements.Table
{
    public class TableColumnDefinition
    {
        public float ConstantSize { get; }
        public float RelativeSize { get; }

        public float Width { get; set; }

        public TableColumnDefinition(float constantSize, float relativeSize)
        {
            ConstantSize = constantSize;
            RelativeSize = relativeSize;
        }
    }
}