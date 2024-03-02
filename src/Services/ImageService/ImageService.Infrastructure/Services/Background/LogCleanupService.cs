using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using static AuthenticationService.Domain.Constants.Constant;

namespace ImageService.Infrastructure.Services.Background
{
    public class LogCleanupService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromDays(2);

        public LogCleanupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Log.CloseAndFlush();
                try
                {
                    Log.Information("Clearing the Log File...");
                    if (FilePaths.txtLogFiles is not null)
                    {
                        foreach (var file in FilePaths.txtLogFiles)
                            File.Delete(file);
                        Log.Information("Log file cleared.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error formed clearing the log file: {ex.Message}");
                }
                Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(_configuration).CreateLogger();
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }
    }
}
