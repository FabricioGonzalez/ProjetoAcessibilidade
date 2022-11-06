using AppUsecases.Project.Entities.FileTemplate;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels;

using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
using ProjectAvalonia.Project.Views;
using ProjectAvalonia.Views;

using ReactiveUI;

using Splat;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;

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
            disposables(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync));

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
    private async Task DoShowDialogAsync(InteractionContext<AddItemViewModel, ExplorerComponentViewModel?> interaction)
    {
        var dialog = new AddItemWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<ExplorerComponentViewModel?>(Locator.Current.GetService<MainWindow>());
        interaction.SetOutput(result);
    }
}
