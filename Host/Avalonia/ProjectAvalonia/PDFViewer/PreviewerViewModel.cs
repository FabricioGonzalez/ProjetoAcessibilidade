using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using AppViewModels.Common;

using Avalonia.Threading;

using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

using ReactiveUI;

using Unit = System.Reactive.Unit;

namespace QuestPDF.Previewer
{
    public class PreviewerViewModel : ViewModelBase, IRoutableViewModel
    {
        public IScreen HostScreen
        {
            get; set;
        }
        public string UrlPathSegment { get; } = "PDFPreviewer";
        public DocumentRenderer DocumentRenderer { get; } = new();

        private IDocument? _document;
        public IDocument? Document
        {
            get => _document;
            set
            {
                this.RaiseAndSetIfChanged(ref _document, value);
                UpdateDocument(value);
            }
        }

        private float _currentScroll;
        public float CurrentScroll
        {
            get => _currentScroll;
            set => this.RaiseAndSetIfChanged(ref _currentScroll, value);
        }

        private float _scrollViewportSize;
        public float ScrollViewportSize
        {
            get => _scrollViewportSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _scrollViewportSize, value);
                VerticalScrollbarVisible = value < 1;
            }
        }

        private bool _verticalScrollbarVisible;
        public bool VerticalScrollbarVisible
        {
            get => _verticalScrollbarVisible;
            private set => Dispatcher.UIThread.Post(() => this.RaiseAndSetIfChanged(ref _verticalScrollbarVisible, value));
        }

        public ReactiveCommand<Unit, Unit> ShowPdfCommand
        {
            get;
        }
        public ReactiveCommand<Unit, Unit> ShowDocumentationCommand
        {
            get;
        }
        public ReactiveCommand<Unit, Unit> SponsorProjectCommand
        {
            get;
        }
        public ReactiveCommand<Unit, Unit> PrintCommand
        {
            get;
        }

        public PreviewerViewModel()
        {
            Activator.Deactivated.Subscribe((x) => UnregisterHotReloadHandler());

            ShowPdfCommand = ReactiveCommand.Create(ShowPdf);
            ShowDocumentationCommand = ReactiveCommand.Create(() => OpenLink("https://www.questpdf.com/documentation/api-reference.html"));
            PrintCommand = ReactiveCommand.Create(() => OpenLink("https://github.com/sponsors/QuestPDF"));

        }

        public void UnregisterHotReloadHandler()
        {
            HotReloadManager.UpdateApplicationRequested -= InvalidateDocument;
        }

        private void InvalidateDocument(object? sender, EventArgs e)
        {
            UpdateDocument(Document);
        }

        private Task UpdateDocument(IDocument? document)
        {
            return Task.Run(() => DocumentRenderer.UpdateDocument(document));
        }

        private void ShowPdf()
        {
            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pdf");

            try
            {
                Document?.GeneratePdf(filePath);
            }
            catch (Exception exception)
            {
                new ExceptionDocument(exception).GeneratePdf(filePath);
            }

            OpenLink(filePath);
        }

        private void OpenLink(string path)
        {
            var openBrowserProcess = new Process
            {
                StartInfo = new()
                {
                    UseShellExecute = true,
                    FileName = path
                }
            };

            openBrowserProcess.Start();
        }
    }
}
