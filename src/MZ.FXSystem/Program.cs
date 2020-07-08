using MZ.FXSystem.Services.FX;
using MZ.FXSystem.Services.FX.Backends.BankOfCanada;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace MZ.FXSystem
{
    class Program
    {
        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddLogging();

            services.AddSingleton<IFXServiceBackend, BankOfCanadaFXServiceBackend>();
            services.AddTransient<FxService>();
            services.AddScoped<FXApplication>();

            return services;
        }

        static async Task Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider(true);

            var app = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FXApplication>();
            await app.Run();

            serviceProvider.Dispose();
        }
    }
}
