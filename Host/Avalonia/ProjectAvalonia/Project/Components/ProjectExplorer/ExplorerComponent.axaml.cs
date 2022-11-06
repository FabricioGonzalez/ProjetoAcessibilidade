using AppUsecases.Project.Entities.FileTemplate;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ProjectAvalonia.Project.Views;

using ReactiveUI;

using Splat;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class ExplorerComponent : ReactiveUserControl<ExplorerComponentViewModel>
{
    public TreeView explorerTree => this.FindControl<TreeView>("explorerTreeView");
    /*public TextBlock Text => this.FindControl<TextBlock>("text");*/
    public ExplorerComponent()
    {
       
        ViewModel ??= Locator.Current.GetService<ExplorerComponentViewModel>();
        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
/*            this.WhenAnyValue(x => x.ViewModel.ExplorerItems)
            .Subscribe(x =>
            {
                Text.Text = x.Count.ToString();
            });
*/
            this.OneWayBind(ViewModel,
                vm => vm.ExplorerItems,
                v => v.explorerTree.Items);

        });

        AvaloniaXamlLoader.Load(this);
    }
}
