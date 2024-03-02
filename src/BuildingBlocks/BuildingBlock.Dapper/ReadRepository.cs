using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Data;
using System.Linq.Expressions;

namespace BuildingBlock.Dapper
{
    public class ReadRepository<T, TId> : IReadRepository<T, TId>
          where T : Entity<TId>
          where TId : ValueObject
    {
        private IDbConnection _dbConnection { get; set; }
        private DapperPersistenceConnection _persistenceConnection;

        public ReadRepository(DatabaseConfig databaseConfig, DbContext dbContext)
        {
            if (databaseConfig.ConnectionString != null)
            {
                var connJson = JsonConvert.SerializeObject(databaseConfig.ConnectionString, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                _persistenceConnection = new(dbContext, connJson, 5);
                _dbConnection = _persistenceConnection.GetDapperConnection();
            }
        }

        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAllAsync()
        {
            _dbConnection.Open();

            try
            {
                string tableName = typeof(T).Name;
                var entity = await _dbConnection.QueryAsync<T>($"SELECT * FROM {tableName}s");

                return entity.ToList();
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return null;
            }
            finally { _dbConnection.Close(); }
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            _dbConnection.Open();

            try
            {
                var data = await _dbConnection.GetAllAsync<T>();
                var results = data.AsQueryable().Where(filter).ToList();
                return results;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return null;
            }
            finally { _dbConnection.Close(); }
        }

        public Task<T> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            _dbConnection.Open();
            try
            {
                var data = await _dbConnection.GetAllAsync<T>();
                var results = data.AsQueryable().SingleOrDefault(filter);
                return results;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return null;
            }
            finally { _dbConnection.Close(); }
        }

        public async Task<T> GetByGuidAsync(TId id)
        {
            _dbConnection.Open();

            try
            {
                string tableName = typeof(T).Name;
                var entity = (await _dbConnection.QueryAsync<T>($"SELECT * FROM {tableName}s WHERE Id = @Id", new { Id = id })).FirstOrDefault();
                return entity;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return null;
            }
            finally { _dbConnection.Close(); }
        }

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> expression = null, bool tracking = true, params Expression<Func<T, object>>[] includeEntity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetByGuidAsync(Guid id, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> expression, bool tracking = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> expression = null, bool tracking = true)
        {
            throw new NotImplementedException();
        }
    }
}
