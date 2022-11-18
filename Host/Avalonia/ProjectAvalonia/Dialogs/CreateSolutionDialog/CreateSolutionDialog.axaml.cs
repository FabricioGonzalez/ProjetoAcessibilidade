using Avalonia.Controls;
using Avalonia.ReactiveUI;

using Project.Core.ViewModels.Dialogs;

namespace ProjectAvalonia.Dialogs.CreateSolutionDialog;
public partial class CreateSolutionDialog : ReactiveWindow<CreateSolutionViewModel>
{
    public CreateSolutionDialog()
    {
        InitializeComponent();
    }
}
