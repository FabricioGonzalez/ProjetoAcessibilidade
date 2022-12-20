using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveUI;

namespace AppViewModels.Interactions.Main;
public class AppInterations
{
    public static readonly Interaction<string, string> MessageQueue = new(); 
}
