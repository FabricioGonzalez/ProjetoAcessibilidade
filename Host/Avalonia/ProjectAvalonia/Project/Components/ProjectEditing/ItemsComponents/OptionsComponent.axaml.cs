using System.Collections.ObjectModel;
using System.Reactive.Disposables;

using App.Core.Entities.Solution.Project.AppItem.DataItems.Checkbox;

using Avalonia;
using Avalonia.Controls;

using ReactiveUI;

namespace ProjectAvalonia.Project.Components.ProjectEditing.ItemsComponents;
public partial class OptionsComponent : UserControl, IActivatableView
{
    public static readonly AttachedProperty<ObservableCollection<AppOptionModel>> OptionsProperty =
         AvaloniaProperty.RegisterAttached<OptionsComponent, UserControl, ObservableCollection<AppOptionModel>>(nameof(Options));
    public ObservableCollection<AppOptionModel>? Options
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
