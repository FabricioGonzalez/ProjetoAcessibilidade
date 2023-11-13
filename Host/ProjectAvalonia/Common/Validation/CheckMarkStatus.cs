using Avalonia;

namespace ProjectAvalonia.Common.Validation;

public class CheckMarkStatus
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<CheckMarkStatus, Visual, bool>(name: "IsEnabled");

    public static bool GetIsEnabled(
        Visual obj
    ) => obj.GetValue(property: IsEnabledProperty);

    public static void SetIsEnabled(
        Visual obj
        , bool value
    ) => obj.SetValue(property: IsEnabledProperty, value: value);
}