using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SystemApplication.Services.Contracts;

namespace SystemApplication.Services.AppLocalData;
public class GetAppLocalData
{
    IAppLocalRepository repository;
    public GetAppLocalData(IAppLocalRepository repository)
    {
        this.repository = repository;
    }

    public void CreatePrintableHTML()
    {
    
    }
}
