using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using BuildingBlock.MsSql;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace BuildingBlock.InMemory
{
    public class InMemoryRepository<T, TId> : IInMemoryRepository<T, TId>
        where T : Entity<TId>
        where TId : ValueObject
    {
        private InMemoryPersistenceConnection persistenceConnection;
        private DbContext DbContext;

        public InMemoryRepository(InMemoryConfig memoryConfig, DbContext dbContext, IServiceProvider serviceProvider)
        {
            persistenceConnection = new InMemoryPersistenceConnection(memoryConfig, dbContext);
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
            catch (System.Exception)
            {
                return false;
            }
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
            catch (System.Exception)
            {
                return default;
            }
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
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await Table.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T> GetAsync(TId id) // ????????????????????
        {
            try
            {
                return await Table.Where(t => t.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
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
            catch (System.Exception)
            {
                return default;
            }
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await Table.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                Table.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateAsync(T entity)
        {
            try
            {
                Table.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> SaveChangesAsync() => await DbContext.SaveChangesAsync();

        public Task<bool> DeleteByIdAsync(T entityId)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByGuidAsync(Guid id, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class InMemoryRepository<T> : IInMemoryRepository<T>
       where T : class
    {
        private InMemoryPersistenceConnection persistenceConnection;
        private DbContext DbContext;

        public InMemoryRepository(InMemoryConfig memoryConfig, DbContext dbContext, IServiceProvider serviceProvider)
        {
            persistenceConnection = new InMemoryPersistenceConnection(memoryConfig, dbContext);
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
            catch (System.Exception)
            {
                return false;
            }
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
            catch (System.Exception)
            {
                return default;
            }
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
            catch (System.Exception)
            {
                return null;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                return await Table.ToListAsync();
            }
            catch (Exception)
            {
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
            catch (System.Exception)
            {
                return default;
            }
        }

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                await Table.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(T entity)
        {
            try
            {
                Table.Remove(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateAsync(T entity)
        {
            try
            {
                Table.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> SaveChangesAsync() => await DbContext.SaveChangesAsync();

        public Task<bool> DeleteByIdAsync(T entityId)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByGuidAsync(Guid id, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid entityId)
        {
            throw new NotImplementedException();
        }

        Task<bool> IWriteRepository<T>.UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }
    }
}
