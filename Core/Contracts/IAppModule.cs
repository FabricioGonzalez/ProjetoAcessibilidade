using Microsoft.Extensions.DependencyInjection;

namespace Core.Contracts
{
    public interface IAppModule
    {
        public void Inject(ServiceCollection serviceProvider);
    }
}