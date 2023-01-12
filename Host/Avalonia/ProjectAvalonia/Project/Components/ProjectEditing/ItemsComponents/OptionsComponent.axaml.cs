using System.Collections.ObjectModel;
using System.Reactive.Disposables;

using AppViewModels.Project.States.ProjectItemState.FormItemState;

using Avalonia;
using Avalonia.Controls;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectEditing.ItemsComponents;
public partial class OptionsComponent : UserControl, IActivatableView
{
    public static readonly AttachedProperty<ObservableCollection<OptionsItemState>> OptionsProperty =
         AvaloniaProperty.RegisterAttached<OptionsComponent, UserControl, ObservableCollection<OptionsItemState>>(nameof(Options));
    public ObservableCollection<OptionsItemState>? Options
    {
        get => GetValue(OptionsProperty);
        set => SetValue(OptionsProperty, value);
    }

    public OptionsComponent()
    {
        this.WhenActivated((CompositeDisposable disposables) =>
        {

        });

        InitializeComponent();
    }
}
