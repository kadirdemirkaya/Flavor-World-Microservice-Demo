using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlock.MsSql
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private DatabaseConfig _databaseConfig;
        private Func<string, Task> _eventPublish;
        private string _serviceName;
        public UnitOfWork(DbContext context, DatabaseConfig databaseConfig)
        {
            _context = context;
            _databaseConfig = databaseConfig;
        }

        public UnitOfWork(DbContext context, DatabaseConfig databaseConfig, Func<string, Task> eventPublish, string serviceName)
        {
            _context = context;
            _databaseConfig = databaseConfig;
            _eventPublish = eventPublish;
            _serviceName = serviceName;
        }

        public async Task<int> SaveChangesAsync()
        {
            int result = await _context.SaveChangesAsync();
            if (_serviceName is not null)
                await _eventPublish.Invoke(_serviceName);
            return result;
        }

        public IReadRepository<T> GetReadRepository<T>() where T : class, new()
        {
            return new ReadRepository<T>(_databaseConfig, _context);
        }

        public IWriteRepository<T> GetWriteRepository<T>() where T : class, new()
        {
            return new WriteRepository<T>(_databaseConfig, _context);
        }

        public IReadRepository<T, TId> GetReadRepository<T, TId>() where T : Entity<TId>, new() where TId : ValueObject
        {
            return new ReadRepository<T, TId>(_databaseConfig, _context);
        }

        public IWriteRepository<T, TId> GetWriteRepository<T, TId>() where T : Entity<TId>, new() where TId : ValueObject
        {
            return new WriteRepository<T, TId>(_databaseConfig, _context);
        }
    }
}
