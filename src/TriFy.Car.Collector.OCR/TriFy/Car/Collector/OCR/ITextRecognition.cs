using System;
using System.Text;

namespace TriFy.Car.Collector.OCR
{
    public interface ITextRecognition : IDisposable
    {
        TextInfo GetText(Encoding encoding = null);
    }
}