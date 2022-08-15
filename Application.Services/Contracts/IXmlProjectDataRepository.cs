using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Models;

namespace SystemApplication.Services.Contracts;
public interface IXmlProjectDataRepository
{
    public ItemModel GetModel(string path);
}
