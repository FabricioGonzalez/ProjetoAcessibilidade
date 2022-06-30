using System;

namespace ProjetoAcessibilidade.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
