using Microsoft.Extensions.Hosting;
using BeatTogether.Extensions;
using BeatTogether.Status.Api.Controllers.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BeatTogether.Status.Api.Controllers.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseStatusServer(this IHostBuilder hostBuilder) =>
            hostBuilder
                .ConfigureAppConfiguration()
                .ConfigureServices((hostBuilderContext, services) =>
                    services
                        .Configure<StatusConfiguration>(hostBuilderContext.Configuration.GetSection("Status"))
                        .Configure<QuickplayConfiguration>(hostBuilderContext.Configuration.GetSection("Quickplay"))
                );
    }
}
