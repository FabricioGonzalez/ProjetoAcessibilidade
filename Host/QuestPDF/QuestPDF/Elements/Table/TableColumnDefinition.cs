namespace QuestPDF.Elements.Table
{
    public class TableColumnDefinition
    {
        public TableColumnDefinition(
            float constantSize
            , float relativeSize
        )
        {
            ConstantSize = constantSize;
            RelativeSize = relativeSize;
        }

        public float ConstantSize
        {
            get;
        }

        public float RelativeSize
        {
            get;
        }

        public float Width
        {
            get;
            set;
        }
    }
}