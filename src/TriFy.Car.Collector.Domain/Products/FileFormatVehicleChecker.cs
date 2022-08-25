using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriFy.Car.Collector.IO;
using TriFy.Car.Collector.OCR;

namespace TriFy.Car.Collector.Products
{
    public class FileFormatVehicleChecker 
    {

        public virtual void Validate(FileInfo file)
        {
            if(!(file.Extension == FileType.Pdf))
            {
                throw new FormatException("The file format is not recognized.");
            }
        }
    }

    public class BlockFileFormatVehicleChecker : FileFormatVehicleChecker
    {
        private readonly Block _block;
        private readonly string _bookmark;

        public BlockFileFormatVehicleChecker(Block block, string bookmark)
        {
            Guard.Against.Null(block, nameof(block));
            Guard.Against.NullOrWhiteSpace(bookmark, nameof(bookmark));

            _block = block;
            _bookmark = bookmark;
        }

        public override void Validate(FileInfo file)
        {
            base.Validate(file);

            string bookmark;
            using (var engine = OcrEngineFactory.CreateBlockEngine<PdfReader>().Inspect(file))
            {
                bookmark = engine.Analyze(_block).GetText(Encoding.UTF8).Text;
            }

            if (!bookmark.Equals(_bookmark, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new FormatException("The file format is not recognized.");
            }
        }
    }
}
