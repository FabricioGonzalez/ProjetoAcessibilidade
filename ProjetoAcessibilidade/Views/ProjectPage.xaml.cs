using Microsoft.UI.Xaml.Controls;

using ProjetoAcessibilidade.ViewModels;

namespace ProjetoAcessibilidade.Views;

public sealed partial class ProjectPage : Page
{
    public ProjectViewModel ViewModel
    {
        get;
    }

    public ProjectPage()
    {
        ViewModel = App.GetService<ProjectViewModel>();
        InitializeComponent();
    }
}
