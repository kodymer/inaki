using Volo.Abp.Modularity;

namespace TriFy.Car.Collector
{
    [DependsOn(
        typeof(CarCollectorApplicationModule),
        typeof(CarCollectorDomainTestModule)
        )]
    public class CarCollectorApplicationTestModule : AbpModule
    {

    }
}