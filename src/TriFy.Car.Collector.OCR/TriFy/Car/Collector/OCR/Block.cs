using Ardalis.GuardClauses;

namespace TriFy.Car.Collector.OCR
{
    public abstract record class Block
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

        public Block(float x, float y, float width, float height)
        {
            Guard.Against.NegativeOrZero(width, nameof(width));
            Guard.Against.NegativeOrZero(height, nameof(height));

            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
