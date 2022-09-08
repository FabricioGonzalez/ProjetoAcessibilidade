using CommunityToolkit.Mvvm.ComponentModel;

using ProjetoAcessibilidade.Contracts.Store;

using SystemApplication.Services.UIOutputs;

namespace ProjetoAcessibilidade.Stores;
public class ProjectStore : ObservableValidator, IStore<ReportDataOutput>
{
    private ReportDataOutput reportData = new();
    public ReportDataOutput GetData()
    {
        return reportData;
    }
    public void SetData(ReportDataOutput data)
    {
        SetProperty(ref reportData, data);
    }

}
