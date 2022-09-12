using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SystemApplication.Services.UIOutputs;
public class ExplorerItem : NotifierBaseClass
{
    public enum ExplorerItemType { Folder, File };

    private string name = "";
    public string Name
    {
        get => name; set
        {
            name = value;
            NotifyPropertyChanged(nameof(Name));
        }
    }

    private string newName = "";
    public string NewName
    {
        get => newName; set
        {
            newName = value;
            NotifyPropertyChanged(nameof(NewName));
        }
    }

    private bool isEditing = false;
    public bool IsEditing
    {
        get => isEditing; set
        {
            isEditing = value;
            NotifyPropertyChanged(nameof(IsEditing));
        }
    }

    private string path = "";
    public string Path
    {
        get => path; set
        {
            path = value;
            NotifyPropertyChanged(nameof(Path));
        }
    }

    public ExplorerItemType Type
    {
        get; set;
    }

    private ObservableCollection<ExplorerItem> m_children;
    public ObservableCollection<ExplorerItem> Children
    {
        get
        {
            if (m_children == null)
            {
                m_children = new ObservableCollection<ExplorerItem>();
            }
            return m_children;
        }
        set
        {
            m_children = value;
        }
    }

    private bool m_isExpanded;
    public bool IsExpanded
    {
        get
        {
            return m_isExpanded;
        }
        set
        {
            if (m_isExpanded != value)
            {
                m_isExpanded = value;
                NotifyPropertyChanged("IsExpanded");
            }
        }
    }
}
