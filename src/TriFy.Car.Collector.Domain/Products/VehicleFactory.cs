using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace TriFy.Car.Collector.Products
{
    public class VehicleFactory : IVehicleFactory, ITransientDependency
    {
        protected List<IVehicleProvider> Providers => _lazyProviders.Value;
        protected VehicleFactoryOptions Options { get; }
        private readonly Lazy<List<IVehicleProvider>> _lazyProviders;

        public VehicleFactory(
            IOptions<VehicleFactoryOptions> options, 
            IServiceProvider serviceProvider)
        {

            Options = options.Value;

            _lazyProviders = new Lazy<List<IVehicleProvider>>(
            () => Options.Providers
                    .Select(c => serviceProvider.GetRequiredService(c) as IVehicleProvider)
                    .ToList(),
            true
        );
        }

        public Task<Vehicle> CreateAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            Vehicle vehicle = null;

            foreach (var provider in Providers)
            {
                if (provider.TryGet(file, out vehicle))
                {
                    break;
                }
            }

            if(vehicle is null)
            {
                throw new BusinessException($"The vehicle information could not be extracted. Verify that the file {file.Name} complies with the correct format. ");
            }

            return Task.FromResult(vehicle);
           
        }
    }
}
