using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
    public static class Helpers
    {
        public static byte[] LoadEmbeddedResource(
            string resourceName
        )
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name: resourceName);
            using var reader = new BinaryReader(input: stream);

            return reader.ReadBytes(count: (int)stream.Length);
        }

        private static PropertyInfo? ToPropertyInfo<T, TValue>(
            this Expression<Func<T, TValue>> selector
        ) => (selector.Body as MemberExpression)?.Member as PropertyInfo;

        public static string? GetPropertyName<T, TValue>(
            this Expression<Func<T, TValue>> selector
        )
            where TValue : class => selector.ToPropertyInfo()?.Name;

        public static TValue? GetPropertyValue<T, TValue>(
            this T target
            , Expression<Func<T, TValue>> selector
        )
            where TValue : class => selector.ToPropertyInfo()?.GetValue(obj: target) as TValue;

        public static void SetPropertyValue<T, TValue>(
            this T target
            , Expression<Func<T, TValue>> selector
            , TValue value
        )
        {
            var property = selector.ToPropertyInfo() ??
                           throw new Exception(message: "Expected property with getter and setter.");
            property?.SetValue(obj: target, value: value);
        }

        public static string PrettifyName(
            this string text
        ) => Regex.Replace(input: text, pattern: @"([a-z])([A-Z])", replacement: "$1 $2");

        public static void VisitChildren(
            this Element? element
            , Action<Element?> handler
        )
        {
            foreach (var child in element.GetChildren().Where(predicate: x => x != null))
            {
                VisitChildren(element: child, handler: handler);
            }

            handler(obj: element);
        }
    }
}