using System;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using ProjectAvalonia.Common.Extensions;

namespace ProjectAvalonia.Behaviors;

public class PhoneBoxMaskBehavior : Behavior<TextBox>
{
    private readonly CompositeDisposable disposables = new();

    protected override void OnAttachedToVisualTree()
    {
        base.OnAttachedToVisualTree();

        if (AssociatedObject is null)
        {
            return;
        }

        AssociatedObject.OnEvent(routedEvent: InputElement.TextInputEvent)
            .Select(selector: x => x.EventArgs)
            .Do(onNext: x => InsertImplicitZero(textInputEventArgs: x, textBox: AssociatedObject, text: x.Text ?? ""))
            /*.Do(x => Filter(x, AssociatedObject, x.Text))*/
            .Subscribe()
            .DisposeWith(compositeDisposable: disposables);
    }

    protected override void OnDetachedFromVisualTree() => disposables.Dispose();


    private static void Filter(
        RoutedEventArgs arg
        , TextBox tb
        , string? newText
    ) => arg.Handled = true;


    private static bool IsValid(
        string str
        , string currentText
    )
    {
        if (currentText == "" && str == CultureInfo.CurrentUICulture.NumberFormat.NegativeSign)
        {
            return true;
        }

        if (str.Any(predicate: char.IsWhiteSpace))
        {
            return false;
        }

        return decimal.TryParse(s: str, style: NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign
            , provider: CultureInfo.CurrentUICulture, result: out _);
    }

    private static void InsertImplicitZero(
        TextInputEventArgs textInputEventArgs
        , TextBox textBox
        , string text
    )
    {
        if (textBox.Text.Length <= 10)
        {
            textInputEventArgs.Text = $"({textBox.Text[..2]}){textBox.Text[2..6]}-{textBox.Text[6..10]}";
        }
        else
        {
            textInputEventArgs.Text = $"({textBox.Text[..2]}){textBox.Text[2..7]}-{textBox.Text[7..11]}";
        }
    }
}