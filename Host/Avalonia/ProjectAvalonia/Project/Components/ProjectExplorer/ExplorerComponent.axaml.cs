using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ReactiveUI;

using Splat;

using System;
using System.Diagnostics;
using System.Reactive.Linq;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class ExplorerComponent : ReactiveUserControl<ExplorerComponentViewModel>
{

    public ExplorerComponent()
    {
        ViewModel = Locator.Current.GetService<ExplorerComponentViewModel>();

      
        this.WhenActivated(disposables =>
        {
            DataContext = ViewModel;

            this.WhenAnyValue(x => x.ViewModel.ExplorerItems)
             .Subscribe(items =>
             {
                 Debug.WriteLine(items.Count);
             });

        });

        AvaloniaXamlLoader.Load(this);
    }
}
