using HttpService.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDBService.Interfaces;
using PastbinService.Config;
using PastebinService.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace PastebinAnalysis
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            await provider.GetService<App>().Run();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton(new LoggerFactory()
                .AddConsole()
                .AddDebug());
            services.AddLogging();

            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{Directory.GetCurrentDirectory()}/Config/")
                .AddJsonFile("app-settings.json", false)
                .Build();
            services.AddOptions();

            services.Configure<MongoDBService.Config.MongoDBSettings>(configuration.GetSection("MongoDBSettings"));
            services.Configure<PastbinSettings>(configuration.GetSection("PastebinSettings"));

            services.AddSingleton<IMongoDBService, MongoDBService.Services.MongoDBService>();
            services.AddScoped<IHttpService, HttpService.Services.HttpService>();
            services.AddScoped<IPastebinService, PastebinService.Services.PastebinService>();
            services.AddSingleton<IPastebinAnalyzerService, PastebinService.Services.PastebinAnalyzerService>();

            services.AddTransient<App>();
        }
    }
}
