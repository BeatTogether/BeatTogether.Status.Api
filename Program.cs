using BeatTogether.Extensions;
using BeatTogether.Status.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BeatTogether.DedicatedServer.Interface;
using Autobus;
using BeatTogether.MasterServer.Interface.ApiInterface.Abstractions;

namespace BeatTogether.Status.Api
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutobus()
                .ConfigureAppConfiguration(true)
                .ConfigureWebHostDefaults(webHostBuilder =>
                    webHostBuilder
                        .ConfigureServices((hostBuilderContext, services) =>
                            services
                                .AddServiceClient<IMatchmakingService>()
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