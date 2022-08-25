using System;
using System.IO;
using System.Text;

namespace TriFy.Car.Collector.OCR
{
    public interface IReader : IStringExtractor, IDisposable
    {
        void Open(FileInfo file);
    }

}
