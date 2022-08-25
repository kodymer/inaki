using System;

namespace TriFy.Car.Collector.OCR
{
    public class OcrEngineFactory
    {
        public static OcrEngine CreateEngine<TReader>()
            where TReader : IReader
        {
            var reader = Activator.CreateInstance<TReader>();
            return new OcrEngine(reader);
        }

        public static OcrBlockEngine CreateBlockEngine<TReader>()
            where TReader : IBlockReader
        {
            var reader = Activator.CreateInstance<TReader>();
            return new OcrBlockEngine(reader);
        }
    }
}
