using AuthenticationService.Domain.Aggregate;
using AuthenticationService.Domain.Aggregate.Entities;
using AuthenticationService.Infrastructure.Interceptors;
using BuildingBlock.Base.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Persistence.Data
{
    public class AuthenticationDbContext : DbContext
    {
        private readonly PublishEventInterceptors _publishEventInterceptors;

        public AuthenticationDbContext()
        {

        }
        public AuthenticationDbContext(DbContextOptions options, PublishEventInterceptors publishEventInterceptors) : base(options)
        {
            _publishEventInterceptors = publishEventInterceptors;
        }
        public AuthenticationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; private set; }
        public DbSet<Role> Roles { get; private set; }
        public DbSet<RoleUser> RoleUsers { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<List<IDomainEvent>>();
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_publishEventInterceptors);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
