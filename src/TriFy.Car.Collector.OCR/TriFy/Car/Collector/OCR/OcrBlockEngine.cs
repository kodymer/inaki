using System;
using System.IO;
using System.Text;

namespace TriFy.Car.Collector.OCR
{
    public record class OcrBlockEngine : IBlockAnalyzer, ITextRecognition
    {

        private OcrEngine _engine;

        private bool disposedValue;

        public OcrBlockEngine(IBlockReader reader)
        {
            _engine = new OcrEngine(reader);
            
        }

        public IBlockAnalyzer Inspect(FileInfo file)
        {
            _engine.Inspect(file);

            return this;
        }

        public ITextRecognition Analyze(Block block)  
        {
            _engine.Reader.As<IBlockReader>().Read(block);

            return this;
        }

        public TextInfo GetText(Encoding encoding = null)
        {
            var block = _engine.Reader.As<IBlockReader>().Block;

            var textInfo = _engine.As<ITextRecognition>().GetText(encoding);
            return new BlockTextInfo(block, textInfo);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _engine.Dispose();
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
