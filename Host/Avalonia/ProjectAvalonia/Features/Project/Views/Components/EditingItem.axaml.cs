using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectAvalonia.Features.Project.Views.Components;
public partial class EditingItem : UserControl
{
    /*    public static readonly AttachedProperty<ItemModel> ItemProperty =
                 AvaloniaProperty.RegisterAttached<EditingItem, UserControl, ItemModel>(
                     nameof(Item));
        public ItemModel? Item
        {
            get => GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }*/
    public ScrollViewer editingScrollViewer => this.FindControl<ScrollViewer>("EditingScrollViewer");

    public EditingItem()
    {
        /* DataContext ??= Locator.Current.GetService<ProjectEditingViewModel>();

         *//* KeyBindings.Add(new Avalonia.Input.KeyBinding()
          {
              Command = ViewModel.SaveItemCommand,
              CommandParameter = ViewModel.Item,
              Gesture = new(Avalonia.Input.Key.S, Avalonia.Input.KeyModifiers.Control)
          });*//*

         this.WhenPropertyChanged(prop => prop.Item)
         .Subscribe(async prop =>
         {
             if (prop.Value is not null)
             {
                 await (DataContext as ProjectEditingViewModel).SetEditingItem(prop.Value.ItemPath);
                 (DataContext as ProjectEditingViewModel).SelectedItem = prop.Value;
                 editingScrollViewer.ScrollToHome();
             }
         });*/

        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
