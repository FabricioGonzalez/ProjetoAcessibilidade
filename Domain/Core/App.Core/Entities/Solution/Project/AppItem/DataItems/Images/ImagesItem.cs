namespace ProjetoAcessibilidade.Core.Entities.Solution.Project.AppItem.DataItems.Images;

public class ImagesItem
{
    public ImagesItem(
        string id
        , string imagePath
        , string imageObservation
    )
    {
        Id = id;
        ImagePath = imagePath;
        ImageObservation = imageObservation;
    }

    public string Id
    {
        get;
        set;
    }

    public string ImagePath
    {
        get;
        set;
    }

    public string ImageObservation
    {
        get;
        set;
    }
}