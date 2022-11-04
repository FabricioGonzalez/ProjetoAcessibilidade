﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Avalonia.Controls;

using Project.Core.Contracts;

using ProjectAvalonia.Views;

namespace ProjectAvalonia.Services;
public class FileDialog : IFileDialog
{
    MainWindow window;
    public FileDialog(MainWindow mainWindow)
    {
        window = mainWindow;
    }
    public async Task<string> SaveFile()
    {
        var dlg = new SaveFileDialog();
        dlg.Filters.Add(new FileDialogFilter() { Name = "Arqivos de Projeto", Extensions = { "prja" } });
        dlg.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });

        var result = await dlg.ShowAsync(window);
        if (result != null)
        {
            var sb = new StringBuilder();
           
                try
                {
                    sb.Append(result);
                }
                catch (Exception ex)
                {
                    string text = string.Format("Error: {0}\n", ex.Message);
                    sb.Append(text);
                }
            
            return sb.ToString();
        }

        return "";
    }
    public async Task<string> GetFolder()
    {
        var dlg = new OpenFolderDialog();

        var result = await dlg.ShowAsync(window);
        if (result != null)
        {
            var sb = new StringBuilder();
           
                try
                {
                    sb.Append(result);
                }
                catch (Exception ex)
                {
                    string text = string.Format("Error: {0}\n", ex.Message);
                    sb.Append(text);
                }
            
            return sb.ToString();
        }

        return "";
    }
    public async Task<string> GetFile()
    {
        var dlg = new OpenFileDialog();
        dlg.Filters.Add(new FileDialogFilter() { Name = "Arqivos de Projeto", Extensions = { "prja" } });
        dlg.Filters.Add(new FileDialogFilter() { Name = "All Files", Extensions = { "*" } });

        var result = await dlg.ShowAsync(window);
        if (result != null)
        {
            string[] fileNames = result;
            var sb = new StringBuilder();
            foreach (string fileName in fileNames)
            {
                try
                {
                        sb.Append(fileName);                    
                }
                catch (Exception ex)
                {
                    string text = string.Format("Error: {0}\n", ex.Message);
                    sb.Append(text);
                }
            }
            return sb.ToString();
        }
        
        return "";
    }
}