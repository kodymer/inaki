using TriFy.Car.Collector.EntityFrameworkCore;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace TriFy.Car.Collector
{
    [DependsOn(
        typeof(CarCollectorDomainModule),
        typeof(CarCollectorEntityFrameworkCoreModule),
        typeof(CarCollectorApplicationContractsModule),
        typeof(AbpDddApplicationModule)
        )]
    public class CarCollectorApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CarCollectorApplicationModule>();
            });

            Configure<AbpJsonOptions>(options =>
            {
                options.UseHybridSerializer = false;
            });
        }
    }
}
