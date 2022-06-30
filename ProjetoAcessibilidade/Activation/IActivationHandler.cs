using System.Threading.Tasks;

namespace ProjetoAcessibilidade.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
