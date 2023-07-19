using Avalonia;
using Avalonia.Media;
using Common.Optional;

namespace ProjectAvalonia.Common.Helpers;

public static class IconExtensions
{
    public static StreamGeometry GetIcon(
        this string iconKey
    )
    {
        /*var icon =*/
        if (Application.Current?
                .Styles
                .TryGetResource(key: iconKey
                    , theme: null, value: out var iconOpen) == true)

        {
            return iconOpen.ToOption()
                .Map(i => i as StreamGeometry)
                .Reduce(() => StreamGeometry.Parse(""));
        }

        return StreamGeometry.Parse("");
        /*.FirstOrDefault(i => i is StyleInclude style
                             && style.Source.AbsolutePath.Equals(
                                 "/Icons/IconsSmall.axaml"));*/

        /*return icon.ToOption()
            .Map(iconResource =>
            {
                (iconResource as StyleInclude).TryGetResource(key: iconKey
                    , theme: null, value: out var iconOpen);

                return iconOpen.ToOption()
                    .Map(i => i as StreamGeometry)
                    .Reduce(() => StreamGeometry.Parse(""));
            })
            .Reduce(() => StreamGeometry.Parse(""));*/
    }
}