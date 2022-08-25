using TriFy.Car.Collector.OCR;

namespace TriFy.Car.Collector.OCR
{
    public interface IBlockReader : IReader
    {
        Block Block { get; set; }

        void Read(Block block);

    }

}

