using System;

namespace TriFy.Car.Collector.OCR
{
    public interface IBlockAnalyzer : IDisposable
    {
        ITextRecognition Analyze(Block block);
    }
}