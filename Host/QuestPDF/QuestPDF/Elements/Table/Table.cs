using System;
using System.Collections.Generic;
using System.Linq;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Table
{
    public class Table
        : Element
            , IStateResettable
    {
        public List<TableColumnDefinition> Columns
        {
            get;
            set;
        } = new();

        public List<TableCell> Cells
        {
            get;
            set;
        } = new();

        public bool ExtendLastCellsToTableBottom
        {
            get;
            set;
        }

        private int StartingRowsCount
        {
            get;
            set;
        }

        private int RowsCount
        {
            get;
            set;
        }

        private int CurrentRow
        {
            get;
            set;
        }

        public void ResetState()
        {
            Cells.ForEach(action: x => x.IsRendered = false);
            CurrentRow = 1;
        }

        public override void Initialize(
            IPageContext pageContext
            , ICanvas canvas
        )
        {
            StartingRowsCount = Cells.Select(selector: x => x.Row).DefaultIfEmpty(defaultValue: 0).Max();
            RowsCount = Cells.Select(selector: x => x.Row + x.RowSpan - 1).DefaultIfEmpty(defaultValue: 0).Max();
            Cells = Cells.OrderBy(keySelector: x => x.Row).ThenBy(keySelector: x => x.Column).ToList();

            base.Initialize(pageContext: pageContext, canvas: canvas);
        }

        public override IEnumerable<Element?> GetChildren() => Cells;

        public override SpacePlan Measure(
            Size availableSpace
        )
        {
            if (!Cells.Any())
            {
                return SpacePlan.FullRender(size: Size.Zero);
            }

            UpdateColumnsWidth(availableWidth: availableSpace.Width);
            var renderingCommands = PlanLayout(availableSpace: availableSpace);

            if (!renderingCommands.Any())
            {
                return SpacePlan.Wrap();
            }

            var width = Columns.Sum(selector: x => x.Width);
            var height = renderingCommands.Max(selector: x => x.Offset.Y + x.Size.Height);
            var tableSize = new Size(width: width, height: height);

            if (tableSize.Width > availableSpace.Width + Size.Epsilon)
            {
                return SpacePlan.Wrap();
            }

            return FindLastRenderedRow(commands: renderingCommands) == StartingRowsCount
                ? SpacePlan.FullRender(size: tableSize)
                : SpacePlan.PartialRender(size: tableSize);
        }

        public override void Draw(
            Size availableSpace
        )
        {
            UpdateColumnsWidth(availableWidth: availableSpace.Width);
            var renderingCommands = PlanLayout(availableSpace: availableSpace);

            foreach (var command in renderingCommands)
            {
                if (command.Measurement.Type == SpacePlanType.FullRender)
                {
                    command.Cell.IsRendered = true;
                }

                if (command.Measurement.Type == SpacePlanType.Wrap)
                {
                    continue;
                }

                Canvas.Translate(vector: command.Offset);
                command.Cell.Draw(availableSpace: command.Size);
                Canvas.Translate(vector: command.Offset.Reverse());
            }

            CurrentRow = FindLastRenderedRow(commands: renderingCommands) + 1;

            var isFullRender = FindLastRenderedRow(commands: renderingCommands) == StartingRowsCount;

            if (isFullRender)
            {
                ResetState();
            }
        }

        private int FindLastRenderedRow(
            ICollection<TableCellRenderingCommand> commands
        ) =>
            commands
                .GroupBy(keySelector: x => x.Cell.Row)
                .Where(predicate: x => x.All(predicate: y =>
                    y.Cell.IsRendered || y.Measurement.Type == SpacePlanType.FullRender))
                .Select(selector: x => x.Key)
                .DefaultIfEmpty(defaultValue: 0)
                .Max();

        private void UpdateColumnsWidth(
            float availableWidth
        )
        {
            var constantWidth = Columns.Sum(selector: x => x.ConstantSize);
            var relativeWidth = Columns.Sum(selector: x => x.RelativeSize);

            var widthPerRelativeUnit = relativeWidth > 0 ? (availableWidth - constantWidth) / relativeWidth : 0;

            foreach (var column in Columns)
            {
                column.Width = column.ConstantSize + column.RelativeSize * widthPerRelativeUnit;
            }
        }

        private ICollection<TableCellRenderingCommand> PlanLayout(
            Size availableSpace
        )
        {
            var columnOffsets = GetColumnLeftOffsets(columns: Columns);

            var commands = GetRenderingCommands();

            if (!commands.Any())
            {
                return commands;
            }

            var tableHeight = commands.Max(selector: cell => cell.Offset.Y + cell.Size.Height);

            if (ExtendLastCellsToTableBottom)
            {
                AdjustLastCellSizes(tableHeight: tableHeight, commands: commands);
            }

            return commands;

            static float[] GetColumnLeftOffsets(
                IList<TableColumnDefinition> columns
            )
            {
                var cellOffsets = new float[columns.Count + 1];
                cellOffsets[0] = 0;

                foreach (var column in Enumerable.Range(start: 1, count: cellOffsets.Length - 1))
                {
                    cellOffsets[column] = columns[index: column - 1].Width + cellOffsets[column - 1];
                }

                return cellOffsets;
            }

            ICollection<TableCellRenderingCommand> GetRenderingCommands()
            {
                var rowBottomOffsets = new DynamicDictionary<int, float>();
                var commands = new List<TableCellRenderingCommand>();

                var cellsToTry = Cells.Where(predicate: x => x.Row + x.RowSpan - 1 >= CurrentRow);
                var currentRow = CurrentRow;
                var maxRenderingRow = RowsCount;

                foreach (var cell in cellsToTry)
                {
                    // update position of previous row
                    if (cell.Row > currentRow)
                    {
                        rowBottomOffsets[key: currentRow] = Math.Max(val1: rowBottomOffsets[key: currentRow]
                            , val2: rowBottomOffsets[key: currentRow - 1]);

                        if (rowBottomOffsets[key: currentRow - 1] > availableSpace.Height + Size.Epsilon)
                        {
                            break;
                        }

                        foreach (var row in Enumerable.Range(start: cell.Row, count: cell.Row - currentRow))
                        {
                            rowBottomOffsets[key: row] = Math.Max(val1: rowBottomOffsets[key: row - 1]
                                , val2: rowBottomOffsets[key: row]);
                        }

                        currentRow = cell.Row;
                    }

                    // cell visibility optimizations
                    if (cell.Row > maxRenderingRow)
                    {
                        break;
                    }

                    // calculate cell position / size
                    var topOffset = rowBottomOffsets[key: cell.Row - 1];

                    var availableWidth = GetCellWidth(cell: cell);
                    var availableHeight = availableSpace.Height - topOffset;
                    var availableCellSize = new Size(width: availableWidth, height: availableHeight);

                    var cellSize = cell.Measure(availableSpace: availableCellSize);

                    // corner case: if cell within the row is not fully rendered, do not attempt to render next row
                    if (cellSize.Type == SpacePlanType.PartialRender)
                    {
                        maxRenderingRow = Math.Min(val1: maxRenderingRow, val2: cell.Row + cell.RowSpan - 1);
                    }

                    // corner case: if cell within the row want to wrap to the next page, do not attempt to render this row
                    if (cellSize.Type == SpacePlanType.Wrap)
                    {
                        maxRenderingRow = Math.Min(val1: maxRenderingRow, val2: cell.Row - 1);
                        continue;
                    }

                    // update position of the last row that cell occupies
                    var bottomRow = cell.Row + cell.RowSpan - 1;
                    rowBottomOffsets[key: bottomRow] = Math.Max(val1: rowBottomOffsets[key: bottomRow]
                        , val2: topOffset + cellSize.Height);

                    // accept cell to be rendered
                    commands.Add(item: new TableCellRenderingCommand
                    {
                        Cell = cell, Measurement = cellSize
                        , Size = new Size(width: availableWidth, height: cellSize.Height)
                        , Offset = new Position(x: columnOffsets[cell.Column - 1], y: topOffset)
                    });
                }

                if (!commands.Any())
                {
                    return commands;
                }

                var maxRow = commands.Select(selector: x => x.Cell).Max(selector: x => x.Row + x.RowSpan);

                foreach (var row in Enumerable.Range(start: CurrentRow, count: maxRow - CurrentRow))
                {
                    rowBottomOffsets[key: row] = Math.Max(val1: rowBottomOffsets[key: row - 1]
                        , val2: rowBottomOffsets[key: row]);
                }

                AdjustCellSizes(commands: commands, rowBottomOffsets: rowBottomOffsets);

                // corner case: reject cell if other cells within the same row are rejected
                return commands.Where(predicate: x => x.Cell.Row <= maxRenderingRow).ToList();
            }

            // corner sase: if two cells end up on the same row (a.Row + a.RowSpan = b.Row + b.RowSpan),
            // bottom edges of their bounding boxes should be at the same level
            static void AdjustCellSizes(
                ICollection<TableCellRenderingCommand> commands
                , DynamicDictionary<int, float> rowBottomOffsets
            )
            {
                foreach (var command in commands)
                {
                    var lastRow = command.Cell.Row + command.Cell.RowSpan - 1;
                    var height = rowBottomOffsets[key: lastRow] - command.Offset.Y;

                    command.Size = new Size(width: command.Size.Width, height: height);
                    command.Offset = new Position(x: command.Offset.X, y: rowBottomOffsets[key: command.Cell.Row - 1]);
                }
            }

            // corner sase: all cells, that are last ones in their respective columns, should take all remaining space
            static void AdjustLastCellSizes(
                float tableHeight
                , ICollection<TableCellRenderingCommand> commands
            )
            {
                var columnsCount = commands.Select(selector: x => x.Cell)
                    .Max(selector: x => x.Column + x.ColumnSpan - 1);

                foreach (var column in Enumerable.Range(start: 1, count: columnsCount))
                {
                    var lastCellInColumn = commands
                        .Where(predicate: x => x.Cell.Column <= column && column < x.Cell.Column + x.Cell.ColumnSpan)
                        .OrderByDescending(keySelector: x => x.Cell.Row + x.Cell.RowSpan)
                        .FirstOrDefault();

                    if (lastCellInColumn == null)
                    {
                        continue;
                    }

                    lastCellInColumn.Size = new Size(width: lastCellInColumn.Size.Width
                        , height: tableHeight - lastCellInColumn.Offset.Y);
                }
            }

            float GetCellWidth(
                TableCell cell
            )
            {
                return columnOffsets[cell.Column + cell.ColumnSpan - 1] - columnOffsets[cell.Column - 1];
            }
        }
    }
}