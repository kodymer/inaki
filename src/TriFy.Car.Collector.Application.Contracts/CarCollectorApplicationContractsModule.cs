using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.SettingManagement;

namespace TriFy.Car.Collector
{
    [DependsOn(
        typeof(CarCollectorDomainSharedModule),
        typeof(AbpObjectExtendingModule)
    )]
    public class CarCollectorApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            CarCollectorDtoExtensions.Configure();
        }
    }
}
