using BeatTogether.Extensions;
using BeatTogether.Status.Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace BeatTogether.Status.Api
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(true)
                .ConfigureWebHostDefaults(webHostBuilder =>
                    webHostBuilder
                        .ConfigureServices((hostBuilderContext, services) =>
                            services
                                .AddOptions()
                                .Configure<StatusConfiguration>(hostBuilderContext.Configuration.GetSection("Status"))
                                .Configure<QuickplayConfiguration>(hostBuilderContext.Configuration.GetSection("Quickplay"))
                                .AddControllers()
                                .AddNewtonsoftJson(options =>
                                {
                                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                                    {
                                        NamingStrategy = new SnakeCaseNamingStrategy()
                                    };
                                })
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
