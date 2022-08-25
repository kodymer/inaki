using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TriFy.Car.Collector.Workers;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace TriFy.Car.Collector.WindowsService.Host
{

    [DependsOn(
        typeof(CarCollectorApplicationModule),
        typeof(CarCollectorWindowsServiceModule),
        typeof(AbpAutofacModule)
    )]
    public class CarCollectorWindowsServiceHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();

            context.Services.AddHostedService<RepuveWorker>();
        }
    }
}
