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
    public class ExportedVehicleProvider : VehicleProvider, ISingletonDependency
    {
        private const string Bookmark = "Resultado de Consulta";

        public ExportedVehicleProvider()
            : base(new BlockFileFormatVehicleChecker(DocumentBlock.Create(1, 10, 700, 100, 20), Bookmark))
        {

        }

        protected override Vehicle GetOrNull(FileInfo f)
        {
            Vehicle vehicle;

            try
            {

                using (var engine = OcrEngineFactory.CreateBlockEngine<PdfReader>().Inspect(f))
                {

                    var brand = engine.Analyze(DocumentBlock.Create(1, 150, 680, 200, 20)).GetText(Encoding.UTF8).Text;
                    var model = engine.Analyze(DocumentBlock.Create(1, 150, 660, 200, 20)).GetText(Encoding.UTF8).Text;
                    var year = engine.Analyze(DocumentBlock.Create(1, 150, 640, 200, 20)).GetText(Encoding.UTF8).Text;
                    var @class = engine.Analyze(DocumentBlock.Create(1, 150, 620, 150, 20)).GetText(Encoding.UTF8).Text;
                    var type = engine.Analyze(DocumentBlock.Create(1, 150, 600, 150, 20)).GetText(Encoding.UTF8).Text;
                    var identificationNumber = engine.Analyze(DocumentBlock.Create(1, 150, 580, 150, 20)).GetText(Encoding.UTF8).Text;
                    var registrationCertificateNumber = engine.Analyze(DocumentBlock.Create(1, 150, 540, 150, 20)).GetText(Encoding.UTF8).Text;
                    var licensePlate = engine.Analyze(DocumentBlock.Create(1, 150, 500, 150, 20)).GetText(Encoding.UTF8).Text;
                    var doorNumber = engine.Analyze(DocumentBlock.Create(1, 150, 480, 150, 20)).GetText(Encoding.UTF8).Text;
                    var version = engine.Analyze(DocumentBlock.Create(1, 150, 440, 150, 20)).GetText(Encoding.UTF8).Text;
                    var displacement = engine.Analyze(DocumentBlock.Create(1, 150, 420, 150, 20)).GetText(Encoding.UTF8).Text;
                    var cylinderNumber = engine.Analyze(DocumentBlock.Create(1, 150, 400, 150, 20)).GetText(Encoding.UTF8).Text;
                    var axleNumber = engine.Analyze(DocumentBlock.Create(1, 150, 380, 150, 20)).GetText(Encoding.UTF8).Text;
                    var complementaryData = engine.Analyze(DocumentBlock.Create(1, 150, 300, 200, 50)).GetText(Encoding.UTF8).Text;
                    var entity = engine.Analyze(DocumentBlock.Create(1, 150, 210, 150, 20)).GetText(Encoding.UTF8).Text;

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