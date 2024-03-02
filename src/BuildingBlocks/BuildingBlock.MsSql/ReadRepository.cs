﻿using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Models.Base;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;
using BuildingBlock.Base.Configs;
using Serilog;

namespace BuildingBlock.MsSql
{
    public class ReadRepository<T, TId> : IReadRepository<T, TId>
          where T : Entity<TId>
          where TId : ValueObject
    {
        private SqlPersistenceConnection persistenceConnection;
        private DbContext DbContext;

        public ReadRepository(DatabaseConfig databaseConfig, DbContext? dbContext)
        {
            if (databaseConfig.ConnectionString != null)
            {
                var connJson = JsonConvert.SerializeObject(databaseConfig.ConnectionString, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                persistenceConnection = new SqlPersistenceConnection(databaseConfig, dbContext);
            }
            DbContext = persistenceConnection.GetContext;
        }

        public ReadRepository(DatabaseConfig databaseConfig, DbContext? dbContext, IServiceProvider serviceProvider)
        {
            if (databaseConfig.ConnectionString != null)
            {
                var connJson = JsonConvert.SerializeObject(databaseConfig.ConnectionString, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                persistenceConnection = new SqlPersistenceConnection(databaseConfig, dbContext);
            }
            DbContext = persistenceConnection.GetContext;
        }

        private DbSet<T> Table => DbContext.Set<T>();


        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (expression is not null)
                    return await query.AnyAsync(expression);

                if (expression != null)
                    query = query.Where(expression);

                return await query.AnyAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return false;
            }
        }

        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression = null, bool tracking = true)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (expression is not null)
                    return await query.CountAsync(expression);

                if (expression != null)
                    query = query.Where(expression);

                return await query.CountAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return default;
            }
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (includeEntity.Any())
                    foreach (var include in includeEntity)
                        query = query.Include(include);

                if (expression != null)
                    query = query.Where(expression);

                return await query.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return null;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await Table.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return null;
            }
        }

        public async Task<T> GetAsync(TId id) // ?
        {
            try
            {
                return await Table.Where(t => t.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return null;
            }
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (includeEntity.Any())
                    foreach (var include in includeEntity)
                        query = query.Include(include);

                if (expression != null)
                    query = query.Where(expression);

                return await query.SingleOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return default;
            }
        }

        public Task<T> GetByGuidAsync(Guid id, bool tracking = true)
        {
            throw new NotImplementedException();
        }
    }

    public class ReadRepository<T> : IReadRepository<T>
        where T : class
    {
        private SqlPersistenceConnection persistenceConnection;
        private DbContext DbContext;
        private IUnitOfWork _unitOfWork;

        public ReadRepository(DatabaseConfig databaseConfig, DbContext? dbContext)
        {
            if (databaseConfig.ConnectionString != null)
            {
                var connJson = JsonConvert.SerializeObject(databaseConfig.ConnectionString, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                persistenceConnection = new SqlPersistenceConnection(databaseConfig, dbContext, 5);
            }
            DbContext = persistenceConnection.GetContext;
        }

        public ReadRepository(DatabaseConfig databaseConfig, DbContext? dbContext, IServiceProvider serviceProvider)
        {
            if (databaseConfig.ConnectionString != null)
            {
                var connJson = JsonConvert.SerializeObject(databaseConfig.ConnectionString, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                persistenceConnection = new SqlPersistenceConnection(databaseConfig, dbContext, 5);
            }
            DbContext = persistenceConnection.GetContext;
        }

        private DbSet<T> Table => DbContext.Set<T>();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (expression is not null)
                    return await query.AnyAsync(expression);

                if (expression != null)
                    query = query.Where(expression);

                return await query.AnyAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return false;
            }
        }

        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> expression = null, bool tracking = true)
        {

            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (expression is not null)
                    return await query.CountAsync(expression);

                if (expression != null)
                    query = query.Where(expression);

                return await query.CountAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return default;
            }
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (includeEntity.Any())
                    foreach (var include in includeEntity)
                        query = query.Include(include);

                if (expression != null)
                    query = query.Where(expression);

                return await query.ToListAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return default;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            try
            {
                var query = Table.AsQueryable();
                if (!tracking)
                    query = query.AsNoTracking();

                if (includeEntity.Any())
                    foreach (var include in includeEntity)
                        query = query.Include(include);

                if (expression != null)
                    query = query.Where(expression);

                return await query.SingleOrDefaultAsync();
            }
            catch (System.Exception ex)
            {
                Log.Error("MsSql Error : " + ex.Message);
                return default;
            }
        }

        public async Task<T> GetByGuidAsync(Guid id, bool tracking = true)
        {
            throw new NotImplementedException();
        }
    }
}
