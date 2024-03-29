﻿using BuildingBlock.Base.Configs;
using BuildingBlock.Base.Extensions;
using BuildingBlock.Base.Models.Base;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BuildingBlock.Redis
{
    public class RedisService<T, TId> : IRedisService<T, TId>
          where T : Entity<TId>
          where TId : ValueObject
    {
        private RedisPersistentConnection persistentConnection;
        private IConnectionMultiplexer connectionFactory;
        private IDatabase _redisDb;
        private bool _disposed;
        private string connectionUrl;
        private RedisConfig RedisConfig;
        private InMemoryConfig InMemoryConfig;

        public RedisService(RedisConfig redisConfig, IServiceProvider serviceProvider)
        {
            RedisConfig = redisConfig;
            if (redisConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(RedisConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionFactory = ConnectionMultiplexer.Connect(JsonConvert.DeserializeObject<string>(connJson));
            }
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig);
            _redisDb = persistentConnection.CreateModel();
        }

        public RedisService(InMemoryConfig memoryConfig, IServiceProvider serviceProvider)
        {
            InMemoryConfig = memoryConfig;
            if (memoryConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(RedisConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionFactory = ConnectionMultiplexer.Connect(JsonConvert.DeserializeObject<string>(connJson));
            }
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig);
            _redisDb = persistentConnection.CreateModel();
        }

        public IDatabase GetDatabase
        {
            get
            {
                lock (TypeLock<T>.Lock)
                {
                    if (_redisDb is null)
                        _redisDb = persistentConnection.CreateModel();
                    return _redisDb;
                }
            }
        }

        public bool IsConnected => persistentConnection != null && persistentConnection.IsConnected;

        public void AddConnection(RedisConfig RedisConfig)
        {
            if (RedisConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(RedisConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionUrl = JsonConvert.DeserializeObject<string>(connJson);
                connectionFactory = ConnectionMultiplexer.Connect(JsonConvert.DeserializeObject<string>(connJson));
            }
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig, 5);
        }

        public void AddConnection(IConfiguration configuration, RedisConfig RedisConfig)
        {
            connectionFactory = ConnectionMultiplexer.Connect($"{configuration["RedisSetting:Host"]}.{configuration["RedisSetting:Port"]}");
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig, 5);
        }

        public bool CheckHealth()
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect(); _redisDb = persistentConnection.CreateModel();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var pingResult = _redisDb.Ping();
                    return pingResult.ToString() == "PONG";
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool Remove(string key)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    return _redisDb.KeyDelete(key);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public void Remove(RedisKey[] keys)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    _redisDb.KeyDelete(keys);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        public bool Exists(string key)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            return _redisDb.KeyExists(key);
        }

        public bool Add<T>(string key, T value, TimeSpan expiresAt) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContent, expiresAt);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool JoinT<T>(string key, T value, TimeSpan expiresAt) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    List<T> dataList;

                    string existingData = _redisDb.StringGet(key);
                    if (existingData == null)
                        dataList = new List<T>();
                    else
                        dataList = JsonConvert.DeserializeObject<List<T>>(existingData);
                    dataList.Add(value);
                    string updatedData = JsonConvert.SerializeObject(dataList);
                    _redisDb.StringSet(key, updatedData);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool Add(string key, object value, TimeSpan expiresAt)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContent, expiresAt);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool AddObjcet(string key, object value, TimeSpan expiresAt)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    var datas = _redisDb.StringGet(key);
                    return _redisDb.StringSet(key, datas + stringContent, expiresAt);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool Update<T>(string key, T value) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContent);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public T Get<T>(string key) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            try
            {
                RedisValue myString = _redisDb.StringGet(key);
                if (myString.HasValue && !myString.IsNullOrEmpty)
                {
                    return JsonExtension.DeserializeJson<T>(myString);
                }

                return null;
            }
            catch (Exception)
            {
                // Log Exception
                return null;
            }
        }

        public List<T> GetList<T>(string key) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            try
            {
                //var server = connectionFactory.GetServer(RedisConfig.Connection);
                var server = connectionFactory.GetServer(connectionUrl);
                var keys = server.Keys(_redisDb.Database, key);
                var keyValues = _redisDb.StringGet(keys.ToArray());

                List<T> values = new List<T>();

                foreach (var redisValue in keyValues)
                {
                    if (redisValue.HasValue && !redisValue.IsNullOrEmpty)
                    {
                        List<T> itemList = JsonExtension.DeserializeJson<List<T>>(redisValue);
                        values.AddRange(itemList);
                    }
                }

                return values;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Stop()
        {
            _disposed = true;
            connectionFactory.Dispose();
        }

        public bool UpdateList<T>(string key, List<T> value) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContents = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContents);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public void AddConnection()
        {
            lock (TypeLock<T>.Lock)
            {
                _redisDb = GetDatabase;
            }
        }

        private static class TypeLock<T>
        {
            public static object Lock { get; } = new object();
        }
    }
    public class RedisService<T> : IRedisService<T> where T : class
    {
        private RedisPersistentConnection persistentConnection;
        private IConnectionMultiplexer connectionFactory;
        private IDatabase _redisDb;
        private bool _disposed;
        private string connectionUrl;
        private RedisConfig RedisConfig;
        private InMemoryConfig InMemoryConfig;

        public RedisService(RedisConfig redisConfig, IServiceProvider serviceProvider)
        {
            RedisConfig = redisConfig;
            if (redisConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(RedisConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionFactory = ConnectionMultiplexer.Connect(JsonConvert.DeserializeObject<string>(connJson));
            }
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig);
            _redisDb = persistentConnection.CreateModel();
        }

        public RedisService(InMemoryConfig memoryConfig, IServiceProvider serviceProvider)
        {
            InMemoryConfig = memoryConfig;
            if (memoryConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(RedisConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionFactory = ConnectionMultiplexer.Connect(JsonConvert.DeserializeObject<string>(connJson));
            }
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig);
            _redisDb = persistentConnection.CreateModel();
        }

        public IDatabase GetDatabase
        {
            get
            {
                lock (TypeLock<T>.Lock)
                {
                    if (_redisDb is null)
                        _redisDb = persistentConnection.CreateModel();
                    return _redisDb;
                }
            }
        }

        public bool IsConnected => persistentConnection != null && persistentConnection.IsConnected;

        public void AddConnection(RedisConfig RedisConfig)
        {
            if (RedisConfig.Connection != null)
            {
                var connJson = JsonConvert.SerializeObject(RedisConfig.Connection, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });
                connectionUrl = JsonConvert.DeserializeObject<string>(connJson);
                connectionFactory = ConnectionMultiplexer.Connect(JsonConvert.DeserializeObject<string>(connJson));
            }
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig, 5);
        }

        public void AddConnection(IConfiguration configuration, RedisConfig RedisConfig)
        {
            connectionFactory = ConnectionMultiplexer.Connect($"{configuration["RedisSetting:Host"]}.{configuration["RedisSetting:Port"]}");
            persistentConnection = new RedisPersistentConnection(connectionFactory, RedisConfig, 5);
        }

        public bool CheckHealth()
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect(); _redisDb = persistentConnection.CreateModel();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var pingResult = _redisDb.Ping();
                    return pingResult.ToString() == "PONG";
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public bool Remove(string key)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    return _redisDb.KeyDelete(key);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public void Remove(RedisKey[] keys)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    _redisDb.KeyDelete(keys);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        public bool Exists(string key)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            return _redisDb.KeyExists(key);
        }

        public bool Add<T>(string key, T value, TimeSpan expiresAt) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContent, expiresAt);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool JoinT<T>(string key, T value, TimeSpan expiresAt) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    List<T> dataList;

                    string existingData = _redisDb.StringGet(key);
                    if (existingData == null)
                        dataList = new List<T>();
                    else
                        dataList = JsonConvert.DeserializeObject<List<T>>(existingData);
                    dataList.Add(value);
                    string updatedData = JsonConvert.SerializeObject(dataList);
                    _redisDb.StringSet(key, updatedData);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool Add(string key, object value, TimeSpan expiresAt)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContent, expiresAt);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool AddObjcet(string key, object value, TimeSpan expiresAt)
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    var datas = _redisDb.StringGet(key);
                    return _redisDb.StringSet(key, datas + stringContent, expiresAt);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        }

        public bool Update<T>(string key, T value) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContent = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContent);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public T Get<T>(string key) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            try
            {
                RedisValue myString = _redisDb.StringGet(key);
                if (myString.HasValue && !myString.IsNullOrEmpty)
                {
                    return JsonExtension.DeserializeJson<T>(myString);
                }

                return null;
            }
            catch (Exception)
            {
                // Log Exception
                return null;
            }
        }

        public List<T> GetList<T>(string key) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            try
            {
                //var server = connectionFactory.GetServer(RedisConfig.Connection);
                var server = connectionFactory.GetServer(connectionUrl);
                var keys = server.Keys(_redisDb.Database, key);
                var keyValues = _redisDb.StringGet(keys.ToArray());

                List<T> values = new List<T>();

                foreach (var redisValue in keyValues)
                {
                    if (redisValue.HasValue && !redisValue.IsNullOrEmpty)
                    {
                        List<T> itemList = JsonExtension.DeserializeJson<List<T>>(redisValue);
                        values.AddRange(itemList);
                    }
                }

                return values;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Stop()
        {
            _disposed = true;
            connectionFactory.Dispose();
        }

        public bool UpdateList<T>(string key, List<T> value) where T : class
        {
            if (!persistentConnection.IsConnected)
                persistentConnection.TryConnect();
            lock (TypeLock<T>.Lock)
            {
                try
                {
                    var stringContents = JsonExtension.SerialJson(value);
                    return _redisDb.StringSet(key, stringContents);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public void AddConnection()
        {
            lock (TypeLock<T>.Lock)
            {
                _redisDb = GetDatabase;
            }
        }

        private static class TypeLock<T>
        {
            public static object Lock { get; } = new object();
        }
    }
}
