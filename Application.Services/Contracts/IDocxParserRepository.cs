using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemApplication.Services.Contracts;
public interface IDocxParserRepository
{
    string ParseDocx(string itemPath);
}
