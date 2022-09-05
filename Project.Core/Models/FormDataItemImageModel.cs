using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Contracts;
using Core.Enums;

namespace Projeto.Core.Models;
public class FormDataItemImageModel : IFormDataItemContract
{
    public string Topic
    {
        get;
        set;
    } = "";
    public FormDataItemTypeEnum Type
    {
        get;
        set;
    } = FormDataItemTypeEnum.Images;

    public List<string> images
    {
        get; set;
    } = new();
}
