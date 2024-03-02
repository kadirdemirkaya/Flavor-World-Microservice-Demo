using BuildingBlock.Base.Configs;
using Microsoft.AspNetCore.Builder;

namespace BuildingBlock.Logger
{
    public static class ServiceExtension
    {
        public static WebApplicationBuilder AddFileBuilder(this WebApplicationBuilder app, Action<LogConfig> configure, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var elasticConfiguration = new LogConfig();
            configure(elasticConfiguration);

            app.AddLogSeriLogFile(elasticConfiguration, configuration);

            return app;
        }

        public static WebApplicationBuilder AddElasticBuilder(this WebApplicationBuilder app, Action<LogConfig> configure, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var elasticConfiguration = new LogConfig();
            configure(elasticConfiguration);

            app.AddLogSeriLog(elasticConfiguration, configuration);

            return app;
        }

        public static WebApplicationBuilder AddConsoleBuilder(this WebApplicationBuilder app, Action<LogConfig> configure, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var elasticConfiguration = new LogConfig();
            configure(elasticConfiguration);

            app.AddLogSeriLogConsole(elasticConfiguration, configuration);

            return app;
        }

        public static WebApplicationBuilder AddLogSeriLogSeq(this WebApplicationBuilder app, Action<LogConfig> configure, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            var elasticConfiguration = new LogConfig();
            configure(elasticConfiguration);

            app.AddLogSeriLogConsole(elasticConfiguration, configuration);

            return app;
        }
    }
}
