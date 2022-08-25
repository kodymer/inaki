using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TriFy.Car.Collector.IO;
using TriFy.Car.Collector.Repuve;
using TriFy.Car.Collector.Settings;
using TriFy.Car.Collector.System.IO;
using Volo.Abp;
using Volo.Abp.Settings;

namespace TriFy.Car.Collector.Workers
{
    public class RepuveWorker : BackgroundService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly ILogger<RepuveWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public RepuveWorker(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            ILogger<RepuveWorker> logger
         )
        {
            _application = application;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TriFy.Car.Collector.WindowsService.Host starting at: {time}", DateTimeOffset.Now);

            _application.Initialize(_serviceProvider);

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TriFy.Car.Collector.WindowsService.Host shutdown at: {time}", DateTimeOffset.Now);

            _application.Shutdown();

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            _logger.LogDebug("Checking the existence of the base directory.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var settingsProvider = scope.ServiceProvider.GetRequiredService<ISettingProvider>();
                var baseDirectoryPath = await settingsProvider.GetOrNullAsync(CarCollectorSettings.BasePath);
                var normalDirectoryName = await settingsProvider.GetOrNullAsync(CarCollectorSettings.NormalRelativePath) ?? "normal";
                var errorDirectoryName = await settingsProvider.GetOrNullAsync(CarCollectorSettings.ErrorRelativePath) ?? "error";

                var baseDirectory = new DirectoryInfo(baseDirectoryPath);
                if (!baseDirectory.Exists)
                {
                    baseDirectory.Create();
                }

                var normalDirectory = baseDirectory.GetOrCreateSubdirectory(normalDirectoryName);
                var errorDirectory = baseDirectory.GetOrCreateSubdirectory(errorDirectoryName);

                _logger.LogDebug($"Observing the directory {baseDirectory.FullName}.");

                var repuveAppService = scope.ServiceProvider.GetRequiredService<IRepuveAppService>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {

                        var files = baseDirectory.GetFiles($"*{FileType.Pdf}", SearchOption.TopDirectoryOnly);
                        foreach (var file in files)
                        {
                            _logger.LogInformation($"Starting work on file: {file.FullName}.");

                            var timer = Stopwatch.StartNew();

                            var destinationPath = normalDirectory.FullName;

                            try
                            {

                                await repuveAppService.ProcessAsync(file, stoppingToken);

                            }
                            catch (Exception)
                            {
                                destinationPath = errorDirectory.FullName;
                            }
                            finally
                            {

                                try
                                {
                                    file.MoveTo(Path.Combine(destinationPath, file.Name), true);
                                }
                                catch (IOException e)
                                {
                                    _logger.LogError(e, $"Could not move the file {file.Name}.");
                                }

                                timer.Stop();

                                _logger.LogInformation($"Finishing work. Time elapsed {timer.Elapsed.ToString(@"hh\:mm\:ss")}.");
                            }
                        }

                        await Task.Delay(1000, stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
        }
    }
}
