namespace AppUsecases.App.Models;
public class UF
{
    public string Code
    {
        get; set;
    }
    public string Name
    {
        get; set;
    }

    public UF(string code, string name)
    {
        Code = code;
        Name = name;
    }
}
