using Autobus;
using BeatTogether.Extensions;
using BeatTogether.MasterServer.Interface.ApiInterface;
using BeatTogether.Status.Api.Configuration;
using BeatTogether.Status.Api.DediServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BeatTogether.Status.Api
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(true)
                .UseAutobus()
                .ConfigureWebHostDefaults(webHostBuilder =>
                    webHostBuilder
                        .ConfigureServices((hostBuilderContext, services) =>
                            services
                                .AddHostedService<ServerCache>()
                                .AddServiceClient<IApiInterface>()
                                .AddOptions()
                                .Configure<StatusConfiguration>(hostBuilderContext.Configuration.GetSection("Status"))
                                .Configure<QuickplayConfiguration>(hostBuilderContext.Configuration.GetSection("Quickplay"))
                                .AddControllers()
                        )
                        .Configure(applicationBuilder =>
                            applicationBuilder
                                .UseRouting()
                                .UseEndpoints(endPointRouteBuilder => endPointRouteBuilder.MapControllers())
                        )
                )
                .UseSerilog();
    }
}
