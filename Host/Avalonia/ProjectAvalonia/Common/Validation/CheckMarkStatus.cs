using Avalonia;

namespace ProjectAvalonia.Common.Validation;

public class CheckMarkStatus
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<CheckMarkStatus, Visual, bool>("IsEnabled");

    public static bool GetIsEnabled(Visual obj)
    {
        return obj.GetValue(IsEnabledProperty);
    }

    public static void SetIsEnabled(Visual obj, bool value)
    {
        obj.SetValue(IsEnabledProperty, value);
    }
}
