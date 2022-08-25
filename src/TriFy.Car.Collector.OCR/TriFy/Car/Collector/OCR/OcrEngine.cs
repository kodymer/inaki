using Ardalis.GuardClauses;
using JetBrains.Annotations;
using System;
using System.IO;
using System.Text;

namespace TriFy.Car.Collector.OCR
{
    public record class OcrEngine : ITextRecognition, IDisposable
    {

        protected internal IReader Reader { get; }

        private bool disposedValue;

        protected internal OcrEngine(IReader reader)
        {
            Guard.Against.Null(reader, nameof(reader));

            Reader = reader;
        }

        public ITextRecognition Inspect(FileInfo file)
        {
            Reader.Open(file);

            return this;
        }

        TextInfo ITextRecognition.GetText(Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var unencodedText = Reader.GetString();
            return new TextInfo(encoding, unencodedText, Transform(encoding, unencodedText));
        }

        private string Transform(Encoding encoding, string decodedText)
        {
           return encoding.GetString(Encoding.Convert(Encoding.Default, encoding, Encoding.Default.GetBytes(decodedText)));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Reader.Dispose();
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
