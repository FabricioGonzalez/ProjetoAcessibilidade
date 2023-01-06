﻿using System.IO;
using QuestPDF.Helpers;
using SkiaSharp;

namespace QuestPDF.Drawing
{
    public class XpsCanvas : SkiaDocumentCanvasBase
    {
        public XpsCanvas(Stream stream, DocumentMetadata documentMetadata) 
            : base(SKDocument.CreateXps(stream, documentMetadata.RasterDpi))
        {
            
        }
    }
}