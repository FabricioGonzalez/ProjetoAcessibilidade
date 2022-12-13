﻿namespace AppUsecases.Project.Entities.FileTemplate
{
    public class ExplorerItem
    {
        public string Name
        {
            get; set;
        }
        public string Path
        {
            get; set;
        }
        public bool InEditMode
        {
            get;
            set;
        } = false;
    }

}