using System.Threading.Tasks;

namespace ProjetoAcessibilidade.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
