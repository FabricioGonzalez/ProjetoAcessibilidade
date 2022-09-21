using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWinui.AppCode.AppUtils.ViewModels;
public class ApplicationViewModel
{
    public List<string> UFs
    {
        get; private set;
    } = new()
    {
"AC",
"AL",
"AP",
"AM",
"BA",
"DF",
"CE",
"ES",
"GO",
"MT",
"MS",
"MA",
"MG",
"PA",
"PR",
"PB",
"PE",
"PI",
"RJ",
"RN",
"RS",
"RO",
"RR",
"SC",
"SP",
"SE",
"TO"
    };
}
