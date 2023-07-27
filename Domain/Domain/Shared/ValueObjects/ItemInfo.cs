namespace Domain.Shared.ValueObjects;

public sealed class ItemInfo
{
    private ItemInfo(
        string name
        , string path
    )
    {
        Name = name;
        StoragePath = path;
    }

    public string Name
    {
        get;
        init;
    }

    public string StoragePath
    {
        get;
        init;
    }

    public static ItemInfo Create(
        string path
    ) => new(name: Path.GetFileNameWithoutExtension(path), path: path);
}