using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TriFy.Car.Collector.Products
{
    public interface IVehicleFactory
    {
        Task<Vehicle> CreateAsync(FileInfo file, CancellationToken cancellationToken = default);
    }
}