using System;

namespace ProjectWinUI.Src.Navigation.Contracts;

public interface IPageService
{
    Type GetPageType(
        string key
    );
}