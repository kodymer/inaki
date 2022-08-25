using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TriFy.Car.Collector.OCR;
using TriFy.Car.Collector.Products;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Threading;

namespace TriFy.Car.Collector.Products
{


    public class PrintedVehicleProvider : VehicleProvider, ISingletonDependency
    {
        private const string Bookmark = "www2.repuve.gob.mx:8080/ciudadania/servletconsulta";

        public PrintedVehicleProvider()
            : base(new BlockFileFormatVehicleChecker(DocumentBlock.Create(1, 10, 10, 300, 50), Bookmark))
        {

        }

        protected override Vehicle GetOrNull(FileInfo f)
        {
            Vehicle vehicle;

            try
            {

                using (var engine = OcrEngineFactory.CreateBlockEngine<PdfReader>().Inspect(f))
                {
                    var brand = engine.Analyze(DocumentBlock.Create(1, 210, 420, 400, 20)).GetText(Encoding.UTF8).Text;
                    var model = engine.Analyze(DocumentBlock.Create(1, 210, 400, 400, 20)).GetText(Encoding.UTF8).Text;
                    var year = engine.Analyze(DocumentBlock.Create(1, 210, 360, 400, 20)).GetText(Encoding.UTF8).Text;
                    var @class = engine.Analyze(DocumentBlock.Create(1, 210, 340, 400, 20)).GetText(Encoding.UTF8).Text;
                    var type = engine.Analyze(DocumentBlock.Create(1, 210, 310, 400, 20)).GetText(Encoding.UTF8).Text;
                    var identificationNumber = engine.Analyze(DocumentBlock.Create(1, 220, 280, 400, 20)).GetText(Encoding.UTF8).Text;
                    var registrationCertificateNumber = engine.Analyze(DocumentBlock.Create(1, 220, 240, 400, 20)).GetText(Encoding.UTF8).Text;
                    var licensePlate = engine.Analyze(DocumentBlock.Create(1, 150, 200, 400, 20)).GetText(Encoding.UTF8).Text;
                    var doorNumber = engine.Analyze(DocumentBlock.Create(1, 220, 160, 400, 20)).GetText(Encoding.UTF8).Text;
                    var version = engine.Analyze(DocumentBlock.Create(1, 220, 120, 400, 20)).GetText(Encoding.UTF8).Text;
                    var displacement = engine.Analyze(DocumentBlock.Create(1, 220, 80, 400, 20)).GetText(Encoding.UTF8).Text;
                    var cylinderNumber = engine.Analyze(DocumentBlock.Create(1, 220, 60, 400, 20)).GetText(Encoding.UTF8).Text;
                    var axleNumber = engine.Analyze(DocumentBlock.Create(2, 210, 780, 400, 20)).GetText(Encoding.UTF8).Text;
                    var complementaryData = GetComplementaryData(
                        engine, new DocumentBlock[] {
                            DocumentBlock.Create(2, 210, 730, 400, 10), DocumentBlock.Create(2, 210, 720, 400, 10),
                            DocumentBlock.Create(2, 210, 710, 400, 10), DocumentBlock.Create(2, 210, 690, 400, 10) }, out var complenentaryDataLineCount);
                    var entity = engine.Analyze(DocumentBlock.Create(2, 210, 
                        complenentaryDataLineCount switch { 1 => 600, 2 => 590, 3 => 580, _ => 560 }, 400, 20)).GetText(Encoding.UTF8).Text;

                    var otherData = BuildOtherData(complementaryData, @class, type, identificationNumber, registrationCertificateNumber, doorNumber, 
                        version, displacement, cylinderNumber, axleNumber, entity);

                    vehicle = CreateAsync(model, brand, licensePlate, Convert.ToInt32(year), otherData);
                }
            }
            catch (Exception)
            {
                vehicle = null;
            }

            return vehicle;
        }

        protected string GetComplementaryData(IBlockAnalyzer engine, DocumentBlock[] blocks, out int lineCount)
        {
            lineCount = 0;

            var result = (string)null;

            foreach (var block in blocks)
            {
                if (TryReadComplementaryData(engine, block,  out var value))
                {
                    result += string.Concat(" ", value);
                    lineCount += 1;
                } 
                else
                {
                    break;
                }
            }

            return result;

        }

        protected bool TryReadComplementaryData(IBlockAnalyzer engine, DocumentBlock block, out string value)
        {
            value = engine.Analyze(block).GetText(Encoding.UTF8).Text;
            return !string.IsNullOrEmpty(value);
        }
    }
}
