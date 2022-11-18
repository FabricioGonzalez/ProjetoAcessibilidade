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
            ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)
            .DisposeWith(disposables);

            /*            this.WhenAnyValue(x => x.ViewModel.ExplorerItems)
                        .Subscribe(x =>
                        {
                            Text.Text = x.Count.ToString();
                        });
            */
            this.OneWayBind(ViewModel,
                vm => vm.ExplorerItems,
                v => v.explorerTree.Items)
            .DisposeWith(disposables);

        });

        AvaloniaXamlLoader.Load(this);
    }
    private async Task DoShowDialogAsync(InteractionContext<AddItemViewModel, string?> interaction)
    {
        var dialog = Locator.Current.GetService<AddItemWindow>();

        if (dialog is AddItemWindow dialogVm)
        {
            dialogVm.DataContext = interaction.Input;

            var result = await dialogVm.ShowDialog<string?>(Locator.Current.GetService<MainWindow>());
            interaction.SetOutput(result);
        }
    }


}
