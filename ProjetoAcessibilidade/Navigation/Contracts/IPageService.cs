using System;

namespace ProjetoAcessibilidade.Navigation.Contracts
{
    public interface IPageService
    {
        Type GetPageType(string key);
    }
}
