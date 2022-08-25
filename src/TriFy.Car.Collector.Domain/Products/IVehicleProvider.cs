using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TriFy.Car.Collector.Products
{
    public interface IVehicleProvider
    {
        bool TryGet(FileInfo f, out Vehicle vehicle);
    }
}
