using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using TriFy.Car.Collector.Workers;
using static Microsoft.Extensions.Hosting.Host;

namespace TriFy.Car.Collector.WindowsService.Host
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.Async(c => c.Console())
                .CreateLogger();

            try
            {
                Log.Information("Starting console host.");
                await CreateHostBuilder(args).Build().RunAsync();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return CreateDefaultBuilder(args)
                .UseAutofac()
                .UseSerilog((context, loggerConfiguration) => {
                    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
                })
                .UseWindowsService(options =>
                {
                    options.ServiceName = "TriFy CarCollector Service";
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    //setup your additional configuration sources
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication<CarCollectorWindowsServiceHostModule>();
                });
               
        }
    }
}
