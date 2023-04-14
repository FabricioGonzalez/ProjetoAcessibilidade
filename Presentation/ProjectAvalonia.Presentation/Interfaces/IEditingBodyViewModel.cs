namespace ProjectAvalonia.Presentation.Interfaces;

public interface IEditingBodyViewModel
{
    public List<ILawListViewModel> LawList
    {
        get;
    }

    public List<IFormViewModel> Form
    {
        get;
    }
}