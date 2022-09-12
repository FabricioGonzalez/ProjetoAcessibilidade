using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Contracts;
using Core.Enums;

namespace SystemApplication.Services.UIOutputs;
public class FormDataItemImageModel : NotifierBaseClass, IFormDataItemContract
{
    public FormDataItemTypeEnum Type
    {
        get;
        set;
    } = FormDataItemTypeEnum.Images;

    public string Topic
    {
        get;
        set;
    }

    private ObservableCollection<string> images = new();
    public ObservableCollection<string> Images
    {
        get => images; set => this.SetAtributeValue(ref images, value);
    } 
}
