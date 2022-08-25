using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TriFy.Car.Collector.Repuve
{
    public interface IRepuveAppService
    {
        Task ProcessAsync(FileInfo file, CancellationToken cancellationToken = default);
    }
}