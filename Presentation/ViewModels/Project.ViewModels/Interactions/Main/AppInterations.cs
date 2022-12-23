using App.Core.Entities.App;

using ReactiveUI;

namespace AppViewModels.Interactions.Main;
public class AppInterations
{
    public static readonly Interaction<MessageModel, MessageModel> MessageQueue = new(); 
}
