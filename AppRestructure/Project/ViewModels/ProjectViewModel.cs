using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ReactiveUI;

using Splat;

using SystemApplication.Services.UIOutputs;

namespace AppRestructure.Project.ViewModels;
public class ProjectViewModel : ReactiveObject, IRoutableViewModel
{
    private ReportDataOutput reportData = new();
    public ReportDataOutput ReportData
    {
        get => reportData;
        set => this.RaiseAndSetIfChanged(ref reportData, value);
    }

    public string UrlPathSegment => "project";

    public IScreen HostScreen
    {
        get;
    }

    public ProjectViewModel()
    {
        //HostScreen = screen ?? Locator.Current.GetService<IScreen>();
    }
}
