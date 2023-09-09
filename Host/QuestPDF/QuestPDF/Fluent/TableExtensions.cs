using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Elements.Table;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
    public class TableColumnsDefinitionDescriptor
    {
        public List<TableColumnDefinition> Columns
        {
            get;
        } = new();

        public void ConstantColumn(
            float width
            , Unit unit = Unit.Point
        ) => ComplexColumn(constantWidth: width.ToPoints(unit: unit));

        public void RelativeColumn(
            float width = 1
        ) => ComplexColumn(relativeWidth: width);

        private void ComplexColumn(
            float constantWidth = 0
            , float relativeWidth = 0
        )
        {
            var columnDefinition = new TableColumnDefinition(constantSize: constantWidth, relativeSize: relativeWidth);
            Columns.Add(item: columnDefinition);
        }
    }

    public class TableCellDescriptor
    {
        public TableCellDescriptor(
            ICollection<TableCell> cells
        )
        {
            Cells = cells;
        }

        private ICollection<TableCell> Cells
        {
            get;
        }

        public ITableCellContainer Cell()
        {
            var cell = new TableCell();
            Cells.Add(item: cell);
            return cell;
        }
    }

    public class TableDescriptor
    {
        public List<TableColumnDefinition> Columns
        {
            get;
        }

        private Table HeaderTable
        {
            get;
        } = new();

        private Table ContentTable
        {
            get;
        } = new();

        private Table FooterTable
        {
            get;
        } = new();

        public void ColumnsDefinition(
            Action<TableColumnsDefinitionDescriptor> handler
        )
        {
            var descriptor = new TableColumnsDefinitionDescriptor();
            handler(obj: descriptor);

            HeaderTable.Columns = descriptor.Columns;
            ContentTable.Columns = descriptor.Columns;
            FooterTable.Columns = descriptor.Columns;
        }

        public void ExtendLastCellsToTableBottom() => ContentTable.ExtendLastCellsToTableBottom = true;

        public void Header(
            Action<TableCellDescriptor> handler
        )
        {
            var descriptor = new TableCellDescriptor(cells: HeaderTable.Cells);
            handler(obj: descriptor);
        }

        public void Footer(
            Action<TableCellDescriptor> handler
        )
        {
            var descriptor = new TableCellDescriptor(cells: FooterTable.Cells);
            handler(obj: descriptor);
        }

        public ITableCellContainer Cell()
        {
            var cell = new TableCell();
            ContentTable.Cells.Add(item: cell);
            return cell;
        }

        public IElement CreateElement()
        {
            var container = new Container();

            ConfigureTable(table: HeaderTable);
            ConfigureTable(table: ContentTable);
            ConfigureTable(table: FooterTable);

            container
                .Decoration(handler: decoration =>
                {
                    decoration.Before().Element(child: HeaderTable);
                    decoration.Content().Element(child: ContentTable);
                    decoration.After().Element(child: FooterTable);
                });

            return container;
        }

        private static void ConfigureTable(
            Table table
        )
        {
            if (!table.Columns.Any())
            {
                throw new DocumentComposeException(
                    message:
                    $"Table should have at least one column. Please call the '{nameof(ColumnsDefinition)}' method to define columns.");
            }

            table.PlanCellPositions();
            table.ValidateCellPositions();
        }
    }

    public static class TableExtensions
    {
        public static void Table(
            this IContainer element
            , Action<TableDescriptor> handler
        )
        {
            var descriptor = new TableDescriptor();
            handler(obj: descriptor);
            element.Element(child: descriptor.CreateElement());
        }
    }

    public static class TableCellExtensions
    {
        private static ITableCellContainer TableCell(
            this ITableCellContainer element
            , Action<TableCell> handler
        )
        {
            if (element is TableCell tableCell)
            {
                handler(obj: tableCell);
            }

            return element;
        }

        public static ITableCellContainer Column(
            this ITableCellContainer tableCellContainer
            , uint value
        ) => tableCellContainer.TableCell(handler: x => x.Column = (int)value);

        public static ITableCellContainer ColumnSpan(
            this ITableCellContainer tableCellContainer
            , uint value
        ) => tableCellContainer.TableCell(handler: x => x.ColumnSpan = (int)value);

        public static ITableCellContainer Row(
            this ITableCellContainer tableCellContainer
            , uint value
        ) => tableCellContainer.TableCell(handler: x => x.Row = (int)value);

        public static ITableCellContainer RowSpan(
            this ITableCellContainer tableCellContainer
            , uint value
        ) => tableCellContainer.TableCell(handler: x => x.RowSpan = (int)value);
    }
}