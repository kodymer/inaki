using Microsoft.Extensions.DependencyInjection;
using System;
using TriFy.Car.Collector.Workers;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace TriFy.Car.Collector
{
    [DependsOn(
        typeof(CarCollectorApplicationContractsModule),
        typeof(AbpBackgroundWorkersModule))]
    public class CarCollectorWindowsServiceModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            
        }

    }
}
