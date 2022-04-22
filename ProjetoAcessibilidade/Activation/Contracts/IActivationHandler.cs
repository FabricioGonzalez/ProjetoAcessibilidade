using System.Threading.Tasks;

namespace ProjetoAcessibilidade.Activation.Contracts
{
    public interface IActivationHandler
    {
        bool CanHandle(object args);

        Task HandleAsync(object args);
    }
}
