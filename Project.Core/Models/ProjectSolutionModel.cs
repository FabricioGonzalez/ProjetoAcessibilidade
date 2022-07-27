using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Core.Models;
public class ProjectSolutionModel
{
    public string? FileName
    {
        get; set;
    }
    public string? FilePath
    {
        get; set;
    }
    public string? ParentFolderName
    {
        get; set;
    }    
    public string? ParentFolderPath
    {
        get; set;
    }
    public ReportDataModel? reportData
    {
        get; set;
    }
}
