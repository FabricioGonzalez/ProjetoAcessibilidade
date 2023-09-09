using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
    public class DebuggingState
    {
        public DebuggingState()
        {
            Reset();
        }

        private DebugStackItem Root
        {
            get;
            set;
        }

        private Stack<DebugStackItem> Stack
        {
            get;
            set;
        }

        public void Reset()
        {
            Root = null;
            Stack = new Stack<DebugStackItem>();
        }

        public void RegisterMeasure(
            IElement element
            , Size availableSpace
        )
        {
            if (element.GetType() == typeof(Container))
            {
                return;
            }

            var item = new DebugStackItem
            {
                Element = element, AvailableSpace = availableSpace
            };

            Root ??= item;

            if (Stack.Any())
            {
                Stack.Peek().Stack.Add(item: item);
            }

            Stack.Push(item: item);
        }

        public void RegisterMeasureResult(
            IElement element
            , SpacePlan spacePlan
        )
        {
            if (element.GetType() == typeof(Container))
            {
                return;
            }

            var item = Stack.Pop();

            if (item.Element != element)
            {
                throw new Exception();
            }

            item.SpacePlan = spacePlan;
        }

        public string BuildTrace()
        {
            var builder = new StringBuilder();
            var nestingLevel = 0;

            Traverse(item: Root);
            return builder.ToString();

            void Traverse(
                DebugStackItem item
            )
            {
                var indent = new string(c: ' ', count: nestingLevel * 4);
                var title = item.Element.GetType().Name;

                if (item.Element is DebugPointer debugPointer)
                {
                    title = debugPointer.Target;
                    title += debugPointer.Highlight ? " 🌟" : string.Empty;
                }

                if (item.SpacePlan.Type != SpacePlanType.FullRender)
                {
                    title = "🔥 " + title;
                }

                builder.AppendLine(value: indent + title);
                builder.AppendLine(value: indent + new string(c: '-', count: title.Length));

                builder.AppendLine(value: indent + "Available space: " + item.AvailableSpace);
                builder.AppendLine(value: indent + "Requested space: " + item.SpacePlan);

                foreach (var configuration in GetElementConfiguration(element: item.Element))
                {
                    builder.AppendLine(value: indent + configuration);
                }

                builder.AppendLine();

                nestingLevel++;
                item.Stack.ToList().ForEach(action: Traverse);
                nestingLevel--;
            }

            static IEnumerable<string> GetElementConfiguration(
                IElement element
            )
            {
                if (element is DebugPointer)
                {
                    return Enumerable.Empty<string>();
                }

                return element
                    .GetType()
                    .GetProperties()
                    .Select(selector: x => new
                    {
                        Property = x.Name.PrettifyName(), Value = x.GetValue(obj: element)
                    })
                    .Where(predicate: x => !(x.Value is IElement))
                    .Where(predicate: x => x.Value is string || !(x.Value is IEnumerable))
                    .Where(predicate: x => !(x.Value is TextStyle))
                    .Select(selector: x => $"{x.Property}: {FormatValue(value: x.Value)}");

                string FormatValue(
                    object value
                )
                {
                    const int maxLength = 100;

                    var text = value?.ToString() ?? "-";

                    if (text.Length < maxLength)
                    {
                        return text;
                    }

                    return text.AsSpan(start: 0, length: maxLength).ToString() + "...";
                }
            }
        }
    }
}