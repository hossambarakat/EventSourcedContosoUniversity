using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcedContosoUniversity.Core.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EventSourcedContosoUniversity.Core.ReadModel.Repositories
{
    public class MongoRepository : IReadModelRepository
    {
        private readonly IMongoClient _client;

        private readonly ReadModelSettings _settings;

        protected IMongoDatabase Db => _client.GetDatabase(_settings.MongoDatabase);

        public MongoRepository(IMongoClient client, ReadModelSettings settings)
        {
            _client = client;
            _settings = settings;
        }
        
        public Task Delete<T>(T item) where T : IMongoDocument, new()
        {
            return Db.GetCollection<T>().DeleteOneAsync(x=> x.Id == item.Id);
        }
        
        public Task<T> GetById<T>(Guid id) where T : IMongoDocument, new()
        {
            return Db.GetCollection<T>().Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        //TODO: Use AllAsQueryable
        public Task<List<T>> All<T>() where T : IMongoDocument, new()
        {
            return Db.GetCollection<T>().AsQueryable().ToListAsync();
        }
        
        public Task Add<T>(T item) where T : IMongoDocument, new()
        {
            return Db.GetCollection<T>().InsertOneAsync(item);
        }
        
        public Task Update<T>(T item) where T : IMongoDocument, new()
        {
            return Db.GetCollection<T>().ReplaceOneAsync(x => x.Id == item.Id, item);
        }

        public IQueryable<T> AllAsQueryable<T>() where T : IMongoDocument, new()
        {
            return Db.GetCollection<T>().AsQueryable();
        }
    }
}
