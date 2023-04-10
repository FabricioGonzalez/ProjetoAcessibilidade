using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class CheckMarkVisibilityBehavior : Behavior<PathIcon>
{
    public static readonly StyledProperty<TextBox> OwnerTextBoxProperty =
        AvaloniaProperty.Register<CheckMarkVisibilityBehavior, TextBox>(name: nameof(OwnerTextBox));

    private CompositeDisposable? _disposables;

    [ResolveByName]
    public TextBox OwnerTextBox
    {
        get => GetValue(property: OwnerTextBoxProperty);
        set => SetValue(property: OwnerTextBoxProperty, value: value);
    }

    protected override void OnAttached() =>
        this.WhenAnyValue(property1: x => x.OwnerTextBox)
            .Subscribe(
                onNext: x =>
                {
                    _disposables?.Dispose();

                    if (x is not null)
                    {
                        _disposables = new CompositeDisposable();

                        var hasErrors = OwnerTextBox.GetObservable(property: DataValidationErrors.HasErrorsProperty);
                        var text = OwnerTextBox.GetObservable(property: TextBox.TextProperty);

                        hasErrors.Select(selector: _ => Unit.Default)
                            .Merge(second: text.Select(selector: _ => Unit.Default))
                            .Throttle(dueTime: TimeSpan.FromMilliseconds(value: 100))
                            .ObserveOn(scheduler: RxApp.MainThreadScheduler)
                            .Subscribe(
                                onNext: _ =>
                                {
                                    if (AssociatedObject is not null)
                                    {
                                        AssociatedObject.Opacity =
                                            !DataValidationErrors.GetHasErrors(control: OwnerTextBox) &&
                                            !string.IsNullOrEmpty(value: OwnerTextBox.Text)
                                                ? 1
                                                : 0;
                                    }
                                })
                            .DisposeWith(compositeDisposable: _disposables);
                    }
                });
}