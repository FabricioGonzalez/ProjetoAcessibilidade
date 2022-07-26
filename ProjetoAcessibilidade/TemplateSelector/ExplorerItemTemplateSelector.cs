using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace ProjetoAcessibilidade.TemplateSelector;
    public class ExplorerItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FolderTemplate
        {
            get; set;
        }
        public DataTemplate FileTemplate
        {
            get; set;
        }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var explorerItem = (ExplorerItem)item;
            return explorerItem.Type == ExplorerItem.ExplorerItemType.Folder ? FolderTemplate : FileTemplate;
        }
    }

