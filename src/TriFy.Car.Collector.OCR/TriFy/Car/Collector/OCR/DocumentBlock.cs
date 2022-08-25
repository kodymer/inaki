using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace TriFy.Car.Collector.OCR
{
    public record class DocumentBlock : Block
    {
        public int PageNumber { get; set; }

        public DocumentBlock(int pageNumber, float x, float y, float width, float height)
            : base(x, y, width, height)
        {
            Guard.Against.NegativeOrZero(pageNumber, nameof(pageNumber));

            PageNumber = pageNumber;
        }

        public static DocumentBlock Create(int pageNumber, float x, float y, float width, float height)
            => new(pageNumber, x, y, width, height);
    }
}
