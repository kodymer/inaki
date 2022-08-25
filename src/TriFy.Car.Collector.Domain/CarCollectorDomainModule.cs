using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TriFy.Car.Collector.MultiTenancy;
using TriFy.Car.Collector.Products;
using Volo.Abp.Domain;
using Volo.Abp.Emailing;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;

namespace TriFy.Car.Collector
{
    [DependsOn(
        typeof(CarCollectorDomainSharedModule),
        typeof(AbpEmailingModule),
        typeof(AbpSettingsModule),
        typeof(AbpDddDomainModule)
    )]
    public class CarCollectorDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureMultiTenancy();
            ConfigureVehicleFactory();
#if DEBUG
            ConfigureEmailSender(context);
#endif

        }

        private void ConfigureEmailSender(ServiceConfigurationContext context)
        {
            context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
        }

        private void ConfigureMultiTenancy()
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }

        private void ConfigureVehicleFactory()
        {
            Configure<VehicleFactoryOptions>(options =>
            {
                options.Providers.Add<ExportedVehicleProvider>();
                options.Providers.Add<PrintedVehicleProvider>();
                options.Providers.Add<ChromeExportedVehicleProvider>();
            });
        }
    }
}
