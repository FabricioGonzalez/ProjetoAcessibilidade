using System.Threading.Tasks;

namespace ProjetoAcessibilidade.Navigation.Contracts
{
    public interface IActivationService
    {
        Task ActivateAsync(object activationArgs);
    }
}
