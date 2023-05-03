namespace ProjetoAcessibilidade.Core.Entities.App;

public class MessageModel
{
    public MessageType Type
    {
        get;
        set;
    }

    public string Message
    {
        get;
        set;
    }
}

public enum MessageType
{
    Debug,
    Info,
    Error,
    Warning
}