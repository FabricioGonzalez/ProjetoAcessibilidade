using Avalonia.Collections;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ReactiveUI;
using System.Collections.Generic;

using Splat;

using AppUsecases.Project.Entities.FileTemplate;
using Avalonia.Controls;
using System.Diagnostics;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class ExplorerComponent : ReactiveUserControl<ExplorerComponentViewModel>
{
    #region AttachedProperties
    public static readonly DirectProperty<ExplorerComponent, IEnumerable<FolderItem>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<ExplorerComponent, IEnumerable<FolderItem>>(
            nameof(Items),
            o => o.ViewModel.Items,
            (o, v) => o.ViewModel.Items = new(v));
    /*o => o.ViewModel.ExplorerItems,
            (o, v) => o.ViewModel.ExplorerItems = (IEnumerable<object>)v);
*/

    #endregion

    private IEnumerable<FolderItem> _items = new AvaloniaList<FolderItem>();
    public IEnumerable<FolderItem> Items
    {
        get => _items;
        set => SetAndRaise(ItemsProperty, ref _items, value);
    }

    public ExplorerComponent()
    {


        this.WhenActivated(disposables =>
        {
            ViewModel = Locator.Current.GetService<ExplorerComponentViewModel>();
           /* this.FindControl<Grid>("templateItem").
       PointerPressed += (sender, args) =>
       {
           if (sender is Grid grid)
           {
               var point = args.GetCurrentPoint(grid);
               var x = point.Position.X;
               var y = point.Position.Y;
               if (point.Properties.IsLeftButtonPressed)
               {
                   Debug.WriteLine("Left Button");
               }
               if (point.Properties.IsRightButtonPressed)
               {
                   Debug.WriteLine("Right Button");
               }
           }
       };*/
        });

        AvaloniaXamlLoader.Load(this);
    }
}
