using TriFy.Car.Collector.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace TriFy.Car.Collector
{
    [DependsOn(
        typeof(CarCollectorEntityFrameworkCoreTestModule)
        )]
    public class CarCollectorDomainTestModule : AbpModule
    {

    }
}