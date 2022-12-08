using AppUsecases.Editing.Entities;

using AppViewModels.Dialogs;
using AppViewModels.Interactions.Project;
using AppViewModels.Project;
using AppViewModels.Project.Mappers;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
using ProjectAvalonia.Views;

using ReactiveUI;

using Splat;

using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class ExplorerComponent : ReactiveUserControl<ProjectExplorerViewModel>
{
    public TreeView explorerTree => this.FindControl<TreeView>("explorerTreeView");

    /*public TextBlock Text => this.FindControl<TextBlock>("text");*/

    /*public void FlyoutClosed_PointerPressed(object sender, EventArgs args)
    {
        ViewModel.IsDocumentSolutionEnabled = !ViewModel.IsDocumentSolutionEnabled;

    }*/
    public ExplorerComponent()
    {
        ViewModel ??= Locator.Current.GetService<ProjectExplorerViewModel>();
        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
            this.BindInteraction(
                ViewModel,
                vm => vm.ShowDialog,
                context => DoShowDialogAsync(context))
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel,
                vm => vm.projectExplorerState.ExplorerItems,
                v => v.explorerTree.Items)
            .DisposeWith(disposables);

            ProjectInteractions
            .SelectedProjectPath
            .RegisterHandler(interaction =>
            {
                ViewModel.CurrentOpenProject = interaction.Input;

                interaction.SetOutput(ViewModel.CurrentOpenProject);

            }).DisposeWith(disposables);

            ProjectInteractions
            .RenameFileInteraction
            .RegisterHandler(interaction =>
            {
                var item = ViewModel.explorerOperations.RenameFile(interaction.Input, 
                    ViewModel.projectExplorerState.ExplorerItems
                .ToList());

                Debug.WriteLine(item.Title);

                interaction.SetOutput(item);

            }).DisposeWith(disposables);

            ProjectInteractions
           .RenameFolderInteraction
           .RegisterHandler(interaction =>
           {
               var item = ViewModel.projectExplorerState.ExplorerItems
               .ToList()
               .SearchFolder(interaction.Input);

               Debug.WriteLine(item.Title);

               interaction.SetOutput(item);

           }).DisposeWith(disposables);

            ProjectInteractions
           .DeleteFileInteraction
           .RegisterHandler(interaction =>
           {
               var item = ViewModel.projectExplorerState.ExplorerItems
               .ToList()
               .SearchFile(interaction.Input);

               Debug.WriteLine(item.Title);

               interaction.SetOutput(item);

           }).DisposeWith(disposables);

            ProjectInteractions
           .DeleteFolderInteraction
           .RegisterHandler(interaction =>
           {
               var item = ViewModel.projectExplorerState.ExplorerItems
               .ToList()
               .SearchFolder(interaction.Input);

               Debug.WriteLine(item.Title);

               interaction.SetOutput(item);

           }).DisposeWith(disposables);

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
