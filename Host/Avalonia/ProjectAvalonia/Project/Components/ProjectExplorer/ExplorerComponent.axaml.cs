using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

using App.Core.Entities.App;
using App.Core.Entities.Solution.Explorer;

using AppViewModels.Dialogs;
using AppViewModels.Interactions.Main;
using AppViewModels.Interactions.Project;
using AppViewModels.Main;
using AppViewModels.Project;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using ProjectAvalonia.Project.Components.ProjectExplorer.Dialogs;
using ProjectAvalonia.Views;

using QuestPDF.Previewer;

using ReactiveUI;

using Splat;

namespace ProjectAvalonia.Project.Components.ProjectExplorer;
public partial class ExplorerComponent : ReactiveUserControl<ProjectExplorerViewModel>
{
    public TreeView ExplorerTree => this.FindControl<TreeView>("explorerTreeView");
    public Button PrintButton => this.FindControl<Button>("AppPrintButton");

    public void FlyoutClosed_PointerPressed(object sender, EventArgs args)
    {
        ViewModel.IsDocumentSolutionEnabled = !ViewModel.IsDocumentSolutionEnabled;
    }

    public ExplorerComponent()
    {
        ViewModel ??= Locator.Current.GetService<ProjectExplorerViewModel>();
        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
            ViewModel.PrintDocument = ReactiveCommand.Create<string>((path) =>
            {
                var route = Locator.Current.GetService<PreviewerViewModel>();
                var main = Locator.Current.GetService<MainViewModel>();

                route.HostScreen = main;

                main.Router.Navigate.Execute(route.SetSolutionPath(ViewModel.CurrentOpenProject)).Subscribe();
            }, ViewModel.HasProjectOpened);

            this.BindInteraction(
                ViewModel,
                vm => vm.ShowDialog,
                context => DoShowDialogAsync(context))
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel,
                vm => vm.projectExplorerState.ExplorerItems,
                v => v.ExplorerTree.Items)
            .DisposeWith(disposables);

            ProjectInteractions
            .SelectedProjectPath
            .RegisterHandler(async interaction =>
            {
                ViewModel.CurrentOpenProject = interaction.Input;

                if (!string.IsNullOrEmpty(ViewModel.CurrentOpenProject))
                {
                    var result = await ViewModel.ReadSolution(ViewModel.CurrentOpenProject);

                    if (result is not null)
                    {
                        ViewModel.SolutionModel.FileName = result.FileName;
                        ViewModel.SolutionModel.FilePath = result.FilePath;
                        ViewModel.SolutionModel.ReportData = result.SolutionReportInfo;
                        ViewModel.SolutionModel.ItemGroups = new(result.ItemGroups is not null ? result.ItemGroups : new());
                        ViewModel.SolutionModel.ParentFolderName = result.ParentFolderName;
                        ViewModel.SolutionModel.ParentFolderPath = result.ParentFolderPath;
                    }
                }
                interaction.SetOutput(ViewModel.CurrentOpenProject);

            }).DisposeWith(disposables);

            ProjectInteractions
            .RenameFileInteraction
            .RegisterHandler(async interaction =>
            {
                var item = await ViewModel
                .explorerOperations
                .RenameFile(interaction.Input,
                    ViewModel.projectExplorerState.ExplorerItems
                .ToList());

                interaction.SetOutput(item);

            })
            .DisposeWith(disposables);

            ProjectInteractions
           .RenameFolderInteraction
           .RegisterHandler(async interaction =>
           {
               var item = await ViewModel
                .explorerOperations
                .RenameFolder(interaction.Input,
                    ViewModel.projectExplorerState.ExplorerItems
                .ToList());

               AppInterations
                     .MessageQueue
                     .Handle(new()
                     {
                         Type = MessageType.Info,
                         Message = $"{item.Title} Foi Renomeado"
                     }).Subscribe();

               interaction.SetOutput(item);

           })
           .DisposeWith(disposables);

            ProjectInteractions
           .DeleteFileInteraction
           .RegisterHandler(async interaction =>
           {
               var item = await ViewModel
               .explorerOperations
               .DeleteFile(interaction.Input,
                   ViewModel.projectExplorerState.ExplorerItems
               .ToList());

               interaction.SetOutput(item);

           })
           .DisposeWith(disposables);

            ProjectInteractions
           .DeleteFolderInteraction
           .RegisterHandler(async interaction =>
           {
               var item = await ViewModel
                 .explorerOperations
                 .DeleteFolder(interaction.Input,
                     ViewModel.projectExplorerState.ExplorerItems
                 .ToList());

               interaction.SetOutput(item);

           })
           .DisposeWith(disposables);

        });

        AvaloniaXamlLoader.Load(this);
    }
    private async Task DoShowDialogAsync(InteractionContext<AddItemViewModel, ExplorerItem?> interaction)
    {
        var dialog = Locator.Current.GetService<AddItemWindow>();

        if (dialog is AddItemWindow dialogVm)
        {
            dialogVm.DataContext = interaction.Input;

            dialogVm.Activate();

            var result = await dialogVm.ShowDialog<ExplorerItem?>(Locator.Current.GetService<MainWindow>());
            interaction.SetOutput(result);
        }
    }


}
