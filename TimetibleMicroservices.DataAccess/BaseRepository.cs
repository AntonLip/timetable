using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models.Interfaces;
using TimetibleMicroservices.Models.SettingsClass;

namespace TimetibleMicroservices.DataAccess
{
    class BaseRepository<TModel> : IRepository<TModel, Guid>
        where TModel : IEntity<Guid>
    {

        private readonly MongoDBSettings _mongoDbSettings;
        private readonly MongoClient _mongoClient;
        protected readonly IMongoDatabase _database;

        protected private BaseRepository(IOptions<MongoDBSettings> mongoDbSettings)
        {
            _mongoDbSettings = mongoDbSettings.Value;
            _mongoClient = new MongoClient(_mongoDbSettings.ConnectionString);
            _database = _mongoClient.GetDatabase(_mongoDbSettings.DatabaseName);
        }
        public Task AddAsync(TModel obj, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, TModel obj, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
