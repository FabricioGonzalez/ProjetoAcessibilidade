using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Contracts;
using Core.Enums;

namespace Projeto.Core.Models;
public class FormDataItemObservationModel : IFormDataItemContract
{
    public string Topic
    {
        get;
        set;
    }
    public FormDataItemTypeEnum Type
    {
        get;
        set;
    }
    public string Observation;
}
