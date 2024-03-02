using AuthenticationService.Application.Abstractions;
using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Infrastructure.Persistence.Data;
using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;
using Serilog;

namespace AuthenticationService.Infrastructure.Persistence.Seeds
{
    public class AuthenticationDbContextSeed
    {
        private readonly IHashService _hashService;

        public AuthenticationDbContextSeed(IHashService hashService)
        {
            _hashService = hashService;
        }

        public async Task SeedAsync(AuthenticationDbContext context)
        {
            var policy = CreatePolicy(nameof(AuthenticationDbContext));

            await policy.ExecuteAsync(async () =>
            {
                using (context)
                {
                    //context.Database.Migrate();

                    //if (!context.Users.Any())
                    //{
                    //    await context.Users.AddRangeAsync(GetSeedUsersDatas());
                    //}

                    //if (!context.Roles.Any())
                    //{
                    //    await context.Roles.AddRangeAsync(GetSeedRolesDatas());
                    //}

                    //int dbResult = await context.SaveChangesAsync();
                    //Log.Information("SEED WORK SAVE CHANGES RESULT -=> " + dbResult);
                }
            });
        }

        private List<User> GetSeedUsersDatas()
        {
            //return new()
            //{
            //    new User(){CreatedDate = DateTime.Now,Email = "amenda@gmail.com",FullName = "amenda",Password = _hashService.StringHashingEncrypt("amenda123"),Id = UserId.CreateUnique(),UserStatus = Domain.Aggregate.Enums.UserStatus.Active},
            //    new User(){CreatedDate = DateTime.Now,Email = "fulya@gmail.com",FullName = "fulya",Password = _hashService.StringHashingEncrypt("fulya123"),Id = UserId.CreateUnique(),UserStatus = Domain.Aggregate.Enums.UserStatus.InActive},
            //    new User(){CreatedDate = DateTime.Now,Email = "faruk@gmail.com",FullName = "faruk",Password = _hashService.StringHashingEncrypt("faruk123"),Id = UserId.CreateUnique(),UserStatus = Domain.Aggregate.Enums.UserStatus.Unknown},
            //    new User(){CreatedDate = DateTime.Now,Email = "ferman@gmail.com",FullName = "ferman",Password = _hashService.StringHashingEncrypt("ferman123"),Id = UserId.CreateUnique(),UserStatus = Domain.Aggregate.Enums.UserStatus.Active},
            //    new User(){CreatedDate = DateTime.Now,Email = "selin@gmail.com",FullName = "selin",Password = _hashService.StringHashingEncrypt("selin123"),Id = UserId.CreateUnique(),UserStatus = Domain.Aggregate.Enums.UserStatus.InActive},
            //    new User(){CreatedDate = DateTime.Now,Email = "serkan@gmail.com",FullName = "serkan",Password = _hashService.StringHashingEncrypt("serkan123"),Id = UserId.CreateUnique(),UserStatus = Domain.Aggregate.Enums.UserStatus.Active},
            //};

            return default;
        }

        private List<Role> GetSeedRolesDatas()
        {
            return new()
            {
                 Role.Create(Domain.Aggregate.Enums.RoleEnum.Guest, true),
                 Role.Create(Domain.Aggregate.Enums.RoleEnum.User, true),
                 Role.Create(Domain.Aggregate.Enums.RoleEnum.Moderator, true),
                 Role.Create(Domain.Aggregate.Enums.RoleEnum.Admin, true),
                 Role.Create(Domain.Aggregate.Enums.RoleEnum.Member, true),
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
