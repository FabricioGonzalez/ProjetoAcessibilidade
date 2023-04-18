namespace AppMediator.Contracts;

public interface IMediator : ICommandDispatcher, IQueryDispatcher, INotificationDispatcher
{
}