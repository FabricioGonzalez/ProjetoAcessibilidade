using System;
using System.Linq;
using QuestPDF.Infrastructure;
using SkiaSharp;

namespace QuestPDF.Helpers
{
    public static class Placeholders
    {
        public static readonly Random Random = new();

        #region Word Cache

        private const string CommonParagraph =
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod " +
            "tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim " +
            "veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea " +
            "commodo consequat. Duis aute irure dolor in reprehenderit in voluptate " +
            "velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint " +
            "occaecat cupidatat non proident, sunt in culpa qui officia deserunt " +
            "mollit anim id est laborum.";

        private static readonly string[] LatinWords =
        {
            "exercitationem", "perferendis", "perspiciatis", "laborum", "eveniet", "sunt", "iure", "nam", "nobis", "eum"
            , "cum", "officiis", "excepturi", "odio", "consectetur", "quasi", "aut", "quisquam", "vel", "eligendi"
            , "itaque", "non", "odit", "tempore", "quaerat", "dignissimos", "facilis", "neque", "nihil", "expedita"
            , "vitae", "vero", "ipsum", "nisi", "animi", "cumque", "pariatur", "velit", "modi", "natus", "iusto"
            , "eaque", "sequi", "illo", "sed", "ex", "et", "voluptatibus", "tempora", "veritatis", "ratione"
            , "assumenda", "incidunt", "nostrum", "placeat", "aliquid", "fuga", "provident", "praesentium", "rem"
            , "necessitatibus", "suscipit", "adipisci", "quidem", "possimus", "voluptas", "debitis", "sint"
            , "accusantium", "unde", "sapiente", "voluptate", "qui", "aspernatur", "laudantium", "soluta", "amet", "quo"
            , "aliquam", "saepe", "culpa", "libero", "ipsa", "dicta", "reiciendis", "nesciunt", "doloribus", "autem"
            , "impedit", "minima", "maiores", "repudiandae", "ipsam", "obcaecati", "ullam", "enim", "totam", "delectus"
            , "ducimus", "quis", "voluptates", "dolores", "molestiae", "harum", "dolorem", "quia", "voluptatem"
            , "molestias", "magni", "distinctio", "omnis", "illum", "dolorum", "voluptatum", "ea", "quas", "quam"
            , "corporis", "quae", "blanditiis", "atque", "deserunt", "laboriosam", "earum", "consequuntur", "hic"
            , "cupiditate", "quibusdam", "accusamus", "ut", "rerum", "error", "minus", "eius", "ab", "ad", "nemo"
            , "fugit", "officia", "at", "in", "id", "quos", "reprehenderit", "numquam", "iste", "fugiat", "sit"
            , "inventore", "beatae", "repellendus", "magnam", "recusandae", "quod", "explicabo", "doloremque", "aperiam"
            , "consequatur", "asperiores", "commodi", "optio", "dolor", "labore", "temporibus", "repellat", "veniam"
            , "architecto", "est", "esse", "mollitia", "nulla", "a", "similique", "eos", "alias", "dolore", "tenetur"
            , "deleniti", "porro", "facere", "maxime", "corrupti"
        };

        private static readonly string[] LongLatinWords = LatinWords.Where(predicate: x => x.Length > 8).ToArray();

        #endregion

        #region Text

        private static string RandomWord()
        {
            var index = Random.Next(minValue: 0, maxValue: LatinWords.Length);
            return LatinWords[index];
        }

        private static string LongRandomWord()
        {
            var index = Random.Next(minValue: 0, maxValue: LongLatinWords.Length);
            return LongLatinWords[index];
        }

        private static string RandomWords(
            int min
            , int max
        )
        {
            var length = Random.Next(minValue: min, maxValue: max + 1);

            var words = Enumerable
                .Range(start: 0, count: length)
                .Select(selector: x => RandomWord());

            return string.Join(separator: " ", values: words);
        }

        public static string LoremIpsum() => CommonParagraph;

        public static string Label() => RandomWords(min: 2, max: 3).FirstCharToUpper();
        public static string Sentence() => RandomWords(min: 6, max: 12).FirstCharToUpper() + ".";
        public static string Question() => RandomWords(min: 4, max: 8).FirstCharToUpper() + "?";

        public static string Paragraph()
        {
            var length = Random.Next(minValue: 3, maxValue: 6);

            var sentences = Enumerable
                .Range(start: 0, count: length)
                .Select(selector: x => Sentence());

            return string.Join(separator: " ", values: sentences);
        }

        public static string Paragraphs()
        {
            var length = Random.Next(minValue: 2, maxValue: 5);

            var sentences = Enumerable
                .Range(start: 0, count: length)
                .Select(selector: x => Paragraph());

            return string.Join(separator: "\n", values: sentences);
        }

        public static string Email() =>
            $"{LongRandomWord()}{Random.Next(minValue: 10, maxValue: 99)}@{LongRandomWord()}.com";

        public static string Name() => LongRandomWord().FirstCharToUpper() + " " + LongRandomWord().FirstCharToUpper();

        public static string PhoneNumber() =>
            $"{Random.Next(minValue: 100, maxValue: 999)}-{Random.Next(minValue: 100, maxValue: 999)}-{Random.Next(minValue: 1000, maxValue: 9999)}";

        private static string FirstCharToUpper(
            this string text
        ) => text.First().ToString().ToUpper() + text.Substring(startIndex: 1);

        #endregion

        #region Time

        private static DateTime RandomDate() => System.DateTime.Now - TimeSpan.FromDays(value: Random.NextDouble());

        public static string Time() => RandomDate().ToString(format: "T");
        public static string ShortDate() => RandomDate().ToString(format: "d");
        public static string LongDate() => RandomDate().ToString(format: "D");
        public static string DateTime() => RandomDate().ToString(format: "G");

        #endregion

        #region Numbers

        public static string Integer() => Random.Next(minValue: 0, maxValue: 10_000).ToString();

        public static string Decimal() =>
            (Random.NextDouble() * Random.Next(minValue: 0, maxValue: 100)).ToString(format: "N2");

        public static string Percent() => (Random.NextDouble() * 100).ToString(format: "N0") + "%";

        #endregion

        #region Visual

        private static readonly string[] BackgroundColors =
        {
            Colors.Red.Lighten3, Colors.Pink.Lighten3, Colors.Purple.Lighten3, Colors.DeepPurple.Lighten3
            , Colors.Indigo.Lighten3, Colors.Blue.Lighten3, Colors.LightBlue.Lighten3, Colors.Cyan.Lighten3
            , Colors.Teal.Lighten3, Colors.Green.Lighten3, Colors.LightGreen.Lighten3, Colors.Lime.Lighten3
            , Colors.Yellow.Lighten3, Colors.Amber.Lighten3, Colors.Orange.Lighten3, Colors.DeepOrange.Lighten3
            , Colors.Brown.Lighten3, Colors.Grey.Lighten3, Colors.BlueGrey.Lighten3
        };

        public static string BackgroundColor()
        {
            var index = Random.Next(minValue: 0, maxValue: BackgroundColors.Length);
            return BackgroundColors[index];
        }

        public static string Color()
        {
            var colors = new[]
            {
                Colors.Red.Medium, Colors.Pink.Medium, Colors.Purple.Medium, Colors.DeepPurple.Medium
                , Colors.Indigo.Medium, Colors.Blue.Medium, Colors.LightBlue.Medium, Colors.Cyan.Medium
                , Colors.Teal.Medium, Colors.Green.Medium, Colors.LightGreen.Medium, Colors.Lime.Medium
                , Colors.Yellow.Medium, Colors.Amber.Medium, Colors.Orange.Medium, Colors.DeepOrange.Medium
                , Colors.Brown.Medium, Colors.Grey.Medium, Colors.BlueGrey.Medium
            };

            var index = Random.Next(minValue: 0, maxValue: colors.Length);
            return colors[index];
        }

        public static byte[] Image(
            int width
            , int height
        ) => Image(size: new Size(width: width, height: height));

        public static byte[] Image(
            Size size
        )
        {
            // shuffle corner positions
            var targetPositions = new[]
            {
                new SKPoint(x: 0, y: 0), new SKPoint(x: size.Width, y: 0), new SKPoint(x: 0, y: size.Height)
                , new SKPoint(x: size.Width, y: size.Height)
            };

            var positions = targetPositions
                .OrderBy(keySelector: x => Random.Next())
                .ToList();

            // rand and shuffle colors
            var colors = BackgroundColors
                .OrderBy(keySelector: x => Random.Next())
                .Take(count: 4)
                .Select(selector: SKColor.Parse)
                .ToArray();

            // create image with white background
            var imageInfo = new SKImageInfo(width: (int)size.Width, height: (int)size.Height);
            using var surface = SKSurface.Create(info: imageInfo);

            using var backgroundPaint = new SKPaint
            {
                Color = SKColors.White
            };

            surface.Canvas.DrawRect(x: 0, y: 0, w: size.Width, h: size.Height, paint: backgroundPaint);

            // draw gradient
            SKShader GetForegroundShader(
                int index
            )
            {
                var radius = Math.Max(val1: size.Width, val2: size.Height);
                var color = colors[index];

                return SKShader.CreateRadialGradient(
                    center: positions[index: index], radius: radius,
                    colors: new[] { color, color.WithAlpha(alpha: 0) }, colorPos: new[] { 0, 1f },
                    mode: SKShaderTileMode.Decal);
            }

            using var shaderPaint = new SKPaint
            {
                Shader = SKShader.CreateCompose(
                    shaderA: SKShader.CreateCompose(shaderA: GetForegroundShader(index: 0)
                        , shaderB: GetForegroundShader(index: 1)),
                    shaderB: SKShader.CreateCompose(shaderA: GetForegroundShader(index: 2)
                        , shaderB: GetForegroundShader(index: 3)))
            };

            surface.Canvas.DrawRect(x: 0, y: 0, w: size.Width, h: size.Height, paint: shaderPaint);

            // return result as an image
            surface.Canvas.Save();
            return surface.Snapshot().Encode(format: SKEncodedImageFormat.Jpeg, quality: 90).ToArray();
        }

        #endregion
    }
}