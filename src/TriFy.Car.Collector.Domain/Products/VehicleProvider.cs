using Ardalis.GuardClauses;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Guids;

namespace TriFy.Car.Collector.Products
{
    public abstract class VehicleProvider : IVehicleProvider
    {
        protected FileFormatVehicleChecker FormatFileVehicleChecker { get; }

        public VehicleProvider()
        {

        }

        public VehicleProvider(FileFormatVehicleChecker formatFileVehicleChecker)
        {
            Guard.Against.Null(formatFileVehicleChecker, nameof(formatFileVehicleChecker));

            FormatFileVehicleChecker = formatFileVehicleChecker;

        }

        protected Vehicle CreateAsync(string model, string brand, string licensePlate, int year, string otherData)
        {
            Guard.Against.NullOrEmpty(licensePlate, nameof(licensePlate));

            var vehicle = new Vehicle()
            {
                Brand = brand,
                Model = model,
                LicensePlate = licensePlate,
                Year = year,
                ComplementaryData = otherData
            };

            return vehicle;
        }

        public virtual bool TryGet(FileInfo f, out Vehicle vehicle)
        {
            bool isNotNull = false;

            try
            {
                FormatFileVehicleChecker?.Validate(f);

                vehicle = GetOrNull(f);

                isNotNull = !(vehicle is null);
            }
            catch (Exception)
            {
                vehicle = null;
            }

            return isNotNull;
        }

        protected abstract Vehicle GetOrNull(FileInfo f);

        protected string BuildOtherData(params string[] data)
        {
            return string.Join(" / ", data.Where(d => !string.IsNullOrWhiteSpace(d)));
        }
    }
}
