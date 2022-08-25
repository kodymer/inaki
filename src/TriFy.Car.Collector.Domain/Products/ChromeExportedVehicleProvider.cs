using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TriFy.Car.Collector.OCR;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Threading;

namespace TriFy.Car.Collector.Products
{
    public class ChromeExportedVehicleProvider : VehicleProvider, ISingletonDependency
    {
        private const string Bookmark = "TriFy Chrome Extension";

        public ChromeExportedVehicleProvider()
            : base(new BlockFileFormatVehicleChecker(DocumentBlock.Create(1, 10, 900, 200, 20), Bookmark))
        {

        }

        protected override Vehicle GetOrNull(FileInfo f)
        {
            Vehicle vehicle;

            try
            {

                using (var engine = OcrEngineFactory.CreateBlockEngine<PdfReader>().Inspect(f))
                {

                    var brand = engine.Analyze(DocumentBlock.Create(1, 200, 820, 200, 20)).GetText(Encoding.UTF8).Text;
                    var model = engine.Analyze(DocumentBlock.Create(1, 200, 800, 200, 20)).GetText(Encoding.UTF8).Text;
                    var year = engine.Analyze(DocumentBlock.Create(1, 200, 780, 200, 20)).GetText(Encoding.UTF8).Text;
                    var @class = engine.Analyze(DocumentBlock.Create(1, 200, 760, 200, 20)).GetText(Encoding.UTF8).Text;
                    var type = engine.Analyze(DocumentBlock.Create(1, 200, 740, 200, 20)).GetText(Encoding.UTF8).Text;
                    var identificationNumber = engine.Analyze(DocumentBlock.Create(1, 200, 720, 200, 20)).GetText(Encoding.UTF8).Text;
                    var registrationCertificateNumber = engine.Analyze(DocumentBlock.Create(1, 200, 700, 200, 20)).GetText(Encoding.UTF8).Text;
                    var licensePlate = engine.Analyze(DocumentBlock.Create(1, 200, 680, 200, 20)).GetText(Encoding.UTF8).Text;
                    var doorNumber = engine.Analyze(DocumentBlock.Create(1, 200, 660, 200, 20)).GetText(Encoding.UTF8).Text;
                    var version = engine.Analyze(DocumentBlock.Create(1, 200, 620, 200, 20)).GetText(Encoding.UTF8).Text;
                    var displacement = engine.Analyze(DocumentBlock.Create(1, 200, 600, 200, 20)).GetText(Encoding.UTF8).Text;
                    var cylinderNumber = engine.Analyze(DocumentBlock.Create(1, 200, 580, 200, 20)).GetText(Encoding.UTF8).Text;
                    var axleNumber = engine.Analyze(DocumentBlock.Create(1, 200, 540, 200, 20)).GetText(Encoding.UTF8).Text;
                    var complementaryData = engine.Analyze(DocumentBlock.Create(1, 200, 500, 500, 20)).GetText(Encoding.UTF8).Text;
                    var entity = engine.Analyze(DocumentBlock.Create(1, 200, 420, 200, 20)).GetText(Encoding.UTF8).Text;

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
    }
}