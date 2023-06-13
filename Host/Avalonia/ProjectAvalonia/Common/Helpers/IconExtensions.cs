using System.Linq;

using Avalonia.Markup.Xaml.Styling;
using Avalonia.Media;

using Common.Optional;

namespace ProjectAvalonia.Common.Helpers;
public static class IconExtensions
{
    public static StreamGeometry GetIcon(this string iconKey)
    {
        return App.Current.Styles.FirstOrDefault(i => i is StyleInclude style
               && style.Source.AbsolutePath.Equals("/Icons/IconsSmall.axaml"))
               .ToOption()
               .Map(iconResource =>
               {
                   (iconResource as StyleInclude).TryGetResource(iconKey, out var iconOpen);

                   return iconOpen.ToOption()
                   .Map((i) => i as StreamGeometry)
                   .Reduce(() => StreamGeometry.Parse(""));
               })
               .Reduce(() => StreamGeometry.Parse(""));
    }
}
