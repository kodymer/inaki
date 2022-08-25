
using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace TriFy.Car.Collector.OCR
{
    public record class BlockTextInfo : TextInfo
    {
        public Block Block { get; }

        public BlockTextInfo(Block block, TextInfo textInfo)
            : base(textInfo.Enconding, textInfo.UnencodedText, textInfo.Text)
        {

            Guard.Against.Null(block, nameof(block));

            Block = block;
        }
    }
}
