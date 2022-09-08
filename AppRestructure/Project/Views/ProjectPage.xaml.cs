using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using AppRestructure.Home.ViewModels;
using AppRestructure.Home.Views;
using AppRestructure.Project.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using ReactiveUI;

using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppRestructure.Project.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ProjectPage : Page, IViewFor<ProjectViewModel>
{
    public ProjectPage()
    {
        InitializeComponent();
        ViewModel = new ProjectViewModel();

        this.WhenActivated(disposable =>
        {
            this.Bind(ViewModel, x => x.ReportData.SolutionName,
                x => x.ProjectName.Text)
            .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => x.ReportData.NomeEmpresa, x => x.ProjectCompanyName.Text)
            .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => x.ReportData.Responsavel, x => x.ProjectResponsableName.Text)
             .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => x.ReportData.Endereco, x => x.ProjectReportAddress.Text)
             .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => x.ReportData.Telefone, x => x.ProjectResponsablePhoneNumber.Text)
             .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => x.ReportData.UF, x => x.ProjectReportUF.Text)
             .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => x.ReportData.Email, x => x.ProjectResponsableEmail.Text)
             .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));

            this.Bind(ViewModel,
                x => DateTimeOffset.Parse(x.ReportData.Data), x => x.ProjectReportDate.Date)
             .WhenAnyValue(x => x.Changed)
                .Subscribe(x => Console.WriteLine(x));


        });
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty
  .Register(nameof(ViewModel), typeof(ProjectViewModel), typeof(ProjectPage), new PropertyMetadata(null));
    public ProjectViewModel ViewModel
    {
        get => (ProjectViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ProjectViewModel)value;
    }
}
