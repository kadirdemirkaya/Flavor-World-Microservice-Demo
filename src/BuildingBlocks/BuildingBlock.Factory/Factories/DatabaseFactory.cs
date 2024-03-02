using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Enums;
using BuildingBlock.Base.Models.Base;
using BuildingBlock.Dapper;
using BuildingBlock.MsSql;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.Factory.Factories
{
    public static class DatabaseFactory<T, TId>
           where T : Entity<TId>
           where TId : ValueObject
    {
        public static IWriteRepository<T, TId> CreateForWriteEntity(DatabaseConfig config, DbContext dbContext, IServiceProvider serviceProvider)
        {
            return config.DatabaseType switch
            {
                DatabaseType.MsSQL => new BuildingBlock.MsSql.WriteRepository<T, TId>(config, dbContext, serviceProvider),
                DatabaseType.Dapper => new BuildingBlock.Dapper.WriteRepository<T, TId>(config, dbContext)
            };
        }

        public static IReadRepository<T, TId> CreateForReadEntity(DatabaseConfig config, DbContext dbContext, IServiceProvider serviceProvider)
        {
            return config.DatabaseType switch
            {
                DatabaseType.MsSQL => new BuildingBlock.MsSql.ReadRepository<T, TId>(config, dbContext, serviceProvider),
                DatabaseType.Dapper => new BuildingBlock.Dapper.ReadRepository<T, TId>(config, dbContext)
            };
        }

        public static IDapperService<T, TId> CreateDapperService(DatabaseConfig config, DbContext dbContext, IServiceProvider serviceProvider)
        {
            return config.DatabaseType switch
            {
                DatabaseType.Dapper => new BuildingBlock.Dapper.DapperService<T, TId>(config, dbContext, serviceProvider)
            };
        }

        public static IUnitOfWork CreateUnitOfWork(DatabaseConfig config, DbContext dbContext) => new UnitOfWork(dbContext, config);
    }

    public static class DatabaseFactory<T>
         where T : class
    {
        public static IWriteRepository<T> CreateForClass(DatabaseConfig config, DbContext dbContext, IServiceProvider serviceProvider)
        {
            return config.DatabaseType switch
            {
                DatabaseType.MsSQL => new BuildingBlock.MsSql.WriteRepository<T>(config, dbContext, serviceProvider),
                DatabaseType.Dapper => new BuildingBlock.Dapper.WriteRepository<T>(config, dbContext)
            };
        }

        public static IUnitOfWork CreateUnitOfWork<T>(DatabaseConfig config, DbContext dbContext) => new UnitOfWork(dbContext, config);
    }

    public static class DatabaseFactory
    {
        public static IUnitOfWork CreateUnitOfWork(DatabaseConfig config, DbContext dbContext) => new UnitOfWork(dbContext, config);
        public static IUnitOfWork CreateUnitOfWork<T, TId>(DatabaseConfig config, DbContext dbContext) where T : Entity<TId>
           where TId : ValueObject => new UnitOfWork(dbContext, config);
        public static IUnitOfWork CreateUnitOfWork<T, TId>(DatabaseConfig config, DbContext dbContext, Func<string, Task> eventPub, string serviceName) where T : Entity<TId>
           where TId : ValueObject => new UnitOfWork(dbContext, config, eventPub, serviceName);
    }
}
