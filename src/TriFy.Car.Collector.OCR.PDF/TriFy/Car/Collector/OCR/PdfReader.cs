using System;
using System.IO;
using System.Text;
using Ardalis.GuardClauses;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using TriFy.Car.Collector.IO;

namespace TriFy.Car.Collector.OCR
{
    public class PdfReader : IBlockReader
    {
        private PdfDocument _document;
        private PdfPage _page;
        private ITextExtractionStrategy _strategy;
        
        Block IBlockReader.Block { get; set; }

        private bool disposedValue;

        public PdfReader()
        {

        }

        public void Open(FileInfo file)
        {
            Guard.Against.Null(file, nameof(file));
            FileCheck.Pdf(file, nameof(file));

            _document = new PdfDocument(new iText.Kernel.Pdf.PdfReader(file));
        }

        public void Read(Block block)
        {
            Guard.Against.Null(block, nameof(block));
            
            (this as IBlockReader).Block = block;

            if(block is DocumentBlock documentBlock)
            {
                _page = _document.GetPage(documentBlock.PageNumber);
            } 
            else
            {
                _page = _document.GetFirstPage();
            }            
            
            var rectangle = new Rectangle(block.X, block.Y, block.Width, block.Height);
            var eventFilter = new TextRegionEventFilter(rectangle);
            _strategy = new FilteredTextEventListener(new LocationTextExtractionStrategy(), eventFilter);
        }

        public string GetString()
        {
            var unencodedText = PdfTextExtractor.GetTextFromPage(_page ?? _document.GetFirstPage(), _strategy);
            return unencodedText;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _document.Close();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
