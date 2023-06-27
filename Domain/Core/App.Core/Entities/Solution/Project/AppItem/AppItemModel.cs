using Core.Entities.Solution.Project.AppItem.DataItems.Images;

using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem;
using ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems;

namespace Core.Entities.Solution.Project.AppItem;

public class AppItemModel
{
    public string Id
    {
        get;
        set;
    }

    public string ItemName
    {
        get;
        set;
    }

    public string TemplateName
    {
        get;
        set;
    }

    public IEnumerable<IAppFormDataItemContract> FormData
    {
        get;
        set;
    } = Enumerable.Empty<IAppFormDataItemContract>();

    public IEnumerable<ObservationModel> Observations
    {
        get;
        set;
    } = Enumerable.Empty<ObservationModel>();
    public IEnumerable<ImagesItem> Images
    {
        get;
        set;
    } = Enumerable.Empty<ImagesItem>();
    public IEnumerable<AppLawModel> LawList
    {
        get;
        set;
    } = Enumerable.Empty<AppLawModel>();
}