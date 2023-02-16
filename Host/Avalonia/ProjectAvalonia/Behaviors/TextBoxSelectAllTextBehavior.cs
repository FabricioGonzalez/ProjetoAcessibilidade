using System.Reactive.Disposables;
using Avalonia.Controls;

namespace ProjectAvalonia.Behaviors;

internal class TextBoxSelectAllTextBehavior : AttachedToVisualTreeBehavior<TextBox>
{
	protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
	{
		AssociatedObject?.SelectAll();
	}
}
