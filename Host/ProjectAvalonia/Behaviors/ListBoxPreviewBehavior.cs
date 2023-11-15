using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using ReactiveUI;

namespace ProjectAvalonia.Behaviors;

public class ListBoxPreviewBehavior : DisposingBehavior<ListBox>
{
    /// <summary>
    ///     Defines the <see cref="PreviewItem" /> property.
    /// </summary>
    public static readonly StyledProperty<object?> PreviewItemProperty =
        AvaloniaProperty.Register<ListBoxPreviewBehavior, object?>(name: nameof(PreviewItem));

    public static readonly StyledProperty<int> DelayProperty =
        AvaloniaProperty.Register<ListBoxPreviewBehavior, int>(name: nameof(Delay));

    private CancellationTokenSource _clearItemCts = new();

    public object? PreviewItem
    {
        get => GetValue(property: PreviewItemProperty);
        set => SetValue(property: PreviewItemProperty, value: value);
    }

    public int Delay
    {
        get => GetValue(property: DelayProperty);
        set => SetValue(property: DelayProperty, value: value);
    }

    protected override void OnAttached(
        CompositeDisposable disposables
    )
    {
        if (AssociatedObject is null)
        {
            return;
        }

        Observable.FromEventPattern(target: AssociatedObject, eventName: nameof(AssociatedObject.PointerExited))
            .Subscribe(onNext: _ => ClearPreviewItem(delay: 0))
            .DisposeWith(compositeDisposable: disposables);

        Observable.FromEventPattern<PointerEventArgs>(target: AssociatedObject
                , eventName: nameof(AssociatedObject.PointerMoved))
            .Subscribe(onNext: x =>
            {
                var visual = AssociatedObject.GetVisualAt(p: x.EventArgs.GetPosition(relativeTo: AssociatedObject));

                var listBoxItem = visual.FindAncestorOfType<ListBoxItem>();

                if (listBoxItem is not null)
                {
                    if (listBoxItem.DataContext != PreviewItem)
                    {
                        CancelClear();
                        PreviewItem = listBoxItem.DataContext;
                    }
                }
                else
                {
                    ClearPreviewItem(delay: Delay);
                }
            })
            .DisposeWith(compositeDisposable: disposables);
    }

    private void ClearPreviewItem(
        int delay
    )
    {
        if (delay > 0)
        {
            Observable.Timer(dueTime: TimeSpan.FromMilliseconds(value: delay))
                .ObserveOn(scheduler: RxApp.MainThreadScheduler)
                .Subscribe(onNext: _ => PreviewItem = null, token: _clearItemCts.Token);
        }
        else
        {
            PreviewItem = null;
        }
    }

    private void CancelClear()
    {
        _clearItemCts.Cancel();
        _clearItemCts.Dispose();
        _clearItemCts = new CancellationTokenSource();
    }
}