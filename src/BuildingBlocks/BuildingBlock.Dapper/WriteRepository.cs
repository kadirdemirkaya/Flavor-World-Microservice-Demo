using BuildingBlock.Base.Abstractions;
using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Models.Base;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System.Data;

namespace BuildingBlock.Dapper
{
    public class WriteRepository<T, TId> : IWriteRepository<T, TId>
        where T : Entity<TId>
        where TId : ValueObject
    {
        private IDbConnection _dbConnection { get; set; }
        private DapperPersistenceConnection _persistenceConnection;

        public WriteRepository(DatabaseConfig databaseConfig, DbContext dbContext)
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
        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                int result = await _dbConnection.InsertAsync<T>(entity);
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return false;
            }
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(TId id)
        {
            _dbConnection.Open();

            try
            {
                string tableName = typeof(T).Name;
                var entity = (await _dbConnection.QueryAsync<T>($"SELECT * FROM {tableName}s WHERE Id = @Id", new { Id = id })).FirstOrDefault();

                if (entity == null)
                    return false;

                var keyProperty = entity.GetType().GetProperty("Id");
                var keyValue = keyProperty.GetValue(entity);
                var affectedRows = await _dbConnection.ExecuteAsync($"DELETE FROM {tableName}s WHERE Id = @Id", new { Id = keyValue });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return false;
            }
            finally { _dbConnection.Close(); }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbConnection.Open();

            try
            {
                return await _dbConnection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return false;
            }
            finally { _dbConnection.Close(); }
        }

        public Task<bool> DeleteByIdAsync(T entityId)
        {
            throw new NotImplementedException();
        }

        bool IWriteRepository<T, TId>.UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }

    public class WriteRepository<T> : IWriteRepository<T>
       where T : class
    {
        private IDbConnection _dbConnection { get; set; }
        private DapperPersistenceConnection _persistenceConnection;

        public WriteRepository(DatabaseConfig databaseConfig, DbContext dbContext)
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

        public async Task<bool> CreateAsync(T entity)
        {
            try
            {
                int result = await _dbConnection.InsertAsync<T>(entity);
                return result > 0 ? true : false;
            }
            catch (Exception ex)
            {

                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            _dbConnection.Open();

            try
            {
                string tableName = typeof(T).Name;
                var entity = (await _dbConnection.QueryAsync<T>($"SELECT * FROM {tableName}s WHERE Id = @Id", new { Id = id })).FirstOrDefault();

                if (entity == null)
                    return false;

                var keyProperty = entity.GetType().GetProperty("Id");
                var keyValue = keyProperty.GetValue(entity);
                var affectedRows = await _dbConnection.ExecuteAsync($"DELETE FROM {tableName}s WHERE Id = @Id", new { Id = keyValue });
                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return false;
            }
            finally { _dbConnection.Close(); }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbConnection.Open();

            try
            {
                return await _dbConnection.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                Log.Error("ERROR MESSAGE IN THE DAPPER: " + ex.Message);
                return false;
            }
            finally { _dbConnection.Close(); }
        }
    }
}
