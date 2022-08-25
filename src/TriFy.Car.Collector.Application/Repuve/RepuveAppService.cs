using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TriFy.Car.Collector.Products;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace TriFy.Car.Collector.Repuve
{
    [RemoteService(false)]
    public class RepuveAppService : CarCollectorAppService, IRepuveAppService
    {
        private readonly IRepository<Vehicle, Guid> _repository;
        private readonly IVehicleFactory _vehicleFactory;

        public RepuveAppService(
            IRepository<Vehicle, Guid> repository,
            IVehicleFactory vehicleFactory)
        {
            Check.NotNull(repository, nameof(repository));
            Check.NotNull(vehicleFactory, nameof(vehicleFactory));

            _repository = repository;
            _vehicleFactory = vehicleFactory;
        }

        public async Task ProcessAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            Check.NotNull(file, nameof(file));

            try
            {

                var newVehicle = await _vehicleFactory.CreateAsync(file, cancellationToken);
                if (!(newVehicle is null))
                {
                    var spec = new VehicleSpecification(newVehicle.LicensePlate);
                    var vehicle = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
                    if (!(vehicle is null))
                    {
                        vehicle.Copy(newVehicle);
                        await _repository.UpdateAsync(vehicle, true, cancellationToken);
                    }
                    else
                    {
                        await _repository.InsertAsync(newVehicle, true, cancellationToken);
                    }
                }

            }
            catch (InvalidOperationException e)
            {
                Logger.LogWarning(e, "Invalid operation.");

                throw;
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, "Unexpected error");

                throw;
            }
        }
    }
}
