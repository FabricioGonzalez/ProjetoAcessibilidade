using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Threading;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace ProjectAvalonia.Features.PDFViewer;

public class DocumentRenderer : INotifyPropertyChanged
{
    private ObservableCollection<PreviewPage> _pages = new();

    public IDocument? Document
    {
        get;
        private set;
    }

    public ObservableCollection<PreviewPage> Pages
    {
        get => _pages;
        set
        {
            if (_pages != value)
            {
                _pages = value;
                PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: nameof(Pages)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void UpdateDocument(
        IDocument? document
    )
    {
        Document = document;

        if (document == null)
        {
            return;
        }

        try
        {
            RenderDocument(document: document);
        }
        catch (Exception exception)
        {
            var exceptionDocument = new ExceptionDocument(exception: exception);
            RenderDocument(document: exceptionDocument);
        }
    }

    private void RenderDocument(
        IDocument document
    )
    {
        var canvas = new PreviewerCanvas();

        DocumentGenerator.RenderDocument(canvas: canvas, document: document);

        foreach (var pages in Pages)
        {
            pages?.Picture?.Dispose();
        }

        Dispatcher.UIThread.Post(action: () =>
        {
            Pages.Clear();
            Pages = new ObservableCollection<PreviewPage>(collection: canvas.Pictures);
        });
    }
}