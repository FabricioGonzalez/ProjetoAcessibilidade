namespace ProjectAvalonia.Common.Models.FileItems;

public partial class FileItem
{
    [AutoNotify] private string _filePath = "";
    [AutoNotify] private bool _inEditMode;
    [AutoNotify] private string _name = "";
}