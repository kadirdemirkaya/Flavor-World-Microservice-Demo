using ImageService.Domain.Aggregate;
using ImageService.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Serilog;

namespace ImageService.Infrastructure.Persistence.Seeds
{
    public class ImageDbContextSeed
    {
        public async Task SeedAsync(ImageDbContext context)
        {
            var policy = CreatePolicy(nameof(ImageDbContext));

            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    context.Database.Migrate();

                    //if (!context.Images.Any())
                    //{
                    //    await context.Images.AddRangeAsync(GetSeedImagesDatas());
                    //}

                    int dbResult = await context.SaveChangesAsync();
                    Log.Information("SEED WORK SAVE CHANGES RESULT -=> " + dbResult);
                }
            });
        }

        private List<Image> GetSeedImagesDatas()
        {
            return new List<Image>()
            {
                //Image.Create()
            };
        }

        private AsyncRetryPolicy CreatePolicy(string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        Log.Warning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
