using System.Collections.Generic;
using QuestPDF.Drawing.Exceptions;

namespace QuestPDF.Elements.Table
{
    internal static class TableLayoutPlanner
    {
        public static void ValidateCellPositions(
            this Table table
        ) => ValidateCellPositions(columnsCount: table.Columns.Count, cells: table.Cells);

        private static void ValidateCellPositions(
            int columnsCount
            , ICollection<TableCell> cells
        )
        {
            const string prefix = "Detected issue in table cells configuration.";

            foreach (var cell in cells)
            {
                if (cell.Column < 1)
                {
                    throw new DocumentComposeException(
                        message:
                        $"{prefix} A cell column position should be greater or equal to 1. Got {cell.Column}.");
                }

                if (cell.Row < 1)
                {
                    throw new DocumentComposeException(
                        message: $"{prefix} A cell row position should be greater or equal to 1. Got {cell.Row}.");
                }

                if (cell.Column > columnsCount)
                {
                    throw new DocumentComposeException(
                        message:
                        $"{prefix} Cell starts at column that does not exist. Cell details: {GetCellDetails(cell: cell)}.");
                }

                if (cell.Column + cell.ColumnSpan - 1 > columnsCount)
                {
                    throw new DocumentComposeException(
                        message:
                        $"{prefix} Table cell location is incorrect. Cell spans over columns that do not exist. Cell details: {GetCellDetails(cell: cell)}.");
                }
            }

            string GetCellDetails(
                TableCell cell
            )
            {
                return $"Row {cell.Row}, Column {cell.Column}, RowSpan {cell.RowSpan}, ColumnSpan {cell.ColumnSpan}";
            }
        }
    }
}