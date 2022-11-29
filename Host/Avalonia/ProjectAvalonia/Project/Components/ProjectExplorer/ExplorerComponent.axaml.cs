using AppUsecases.Editing.Entities;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Project;

using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
using ProjectAvalonia.Views;

using ReactiveUI;

using Splat;

using System.Reactive.Disposables;
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
            // In a view
            this.BindInteraction(
                ViewModel,
                vm => vm.ShowDialog,
                context => DoShowDialogAsync(context))
                /*.DisposeWith(disposables)*/;

            this.OneWayBind(ViewModel,
                vm => vm.ExplorerItems,
                v => v.explorerTree.Items)
            .DisposeWith(disposables);

        });

        AvaloniaXamlLoader.Load(this);
    }
    private async Task DoShowDialogAsync(InteractionContext<AddItemViewModel, FileTemplate?> interaction)
    {
        var dialog = Locator.Current.GetService<AddItemWindow>();

        if (dialog is AddItemWindow dialogVm)
        {
            dialogVm.DataContext = interaction.Input;

            dialogVm.Activate();

            var result = await dialogVm.ShowDialog<FileTemplate?>(Locator.Current.GetService<MainWindow>());
            interaction.SetOutput(result);
        }
    }


}
