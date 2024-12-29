using BeatTogether.Extensions;
using BeatTogether.Status.Api.Controllers.Configuration;
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
                                        NamingStrategy = new CamelCaseNamingStrategy()
                                    };
                                })
                        )
                        .Configure(applicationBuilder =>
                            applicationBuilder
	                            .Use((context, next) =>
	                            {
		                            context.Response.Headers.Add("X-Robots-Tag", "noindex, nofollow"); // Tell everyone that we don't want to be indexed
		                            return next(context);
	                            })
								.UseRouting()
                                .UseEndpoints(endPointRouteBuilder => endPointRouteBuilder.MapControllers())
                        )
                )
                .UseSerilog();
    }
}
