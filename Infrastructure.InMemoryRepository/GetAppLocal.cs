using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.InMemoryRepository;

public class GetAppLocal
{
    public string getAppLocal()
    {
        var envPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)!;

        return envPath;
    }
}
