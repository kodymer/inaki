using Volo.Abp.Collections;

namespace TriFy.Car.Collector.Products
{
    public class VehicleFactoryOptions
    {
        public ITypeList<IVehicleProvider> Providers { get; }

        public VehicleFactoryOptions()
        {
            Providers = new TypeList<IVehicleProvider>();
        }
    }
}